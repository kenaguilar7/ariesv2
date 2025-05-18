# AWS Deployment Update Script
$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting AWS deployment update process..." -ForegroundColor Green

# Configuration
$sshKeyPath = "C:\LightsailDefaultKey-us-east-1.pem"
$remoteHost = "ubuntu@35.175.254.171"
$remoteDir = "~/ariesv2"

try {
    # Step 1: Build and Push Docker Images
    Write-Host "`n📦 Building and pushing Docker images..." -ForegroundColor Cyan
    & "$PSScriptRoot\build-and-push-all.ps1"
    if ($LASTEXITCODE -ne 0) { throw "Failed to build and push Docker images!" }

    # Step 2: Update Remote Deployment
    Write-Host "`n🔄 Updating remote deployment..." -ForegroundColor Cyan
    
    # Prepare the update commands
    $updateCommands = @"
cd $remoteDir
docker pull kenaguilar7/ariesv2-api:latest
docker pull kenaguilar7/ariesv2-ui:latest
docker compose down
docker compose up -d
"@

    # Execute update commands on remote server
    $updateCommands | ssh -i $sshKeyPath $remoteHost

    if ($LASTEXITCODE -ne 0) { 
        throw "Failed to update remote deployment!" 
    }

    # Step 3: Verify Deployment
    Write-Host "`n🔍 Verifying deployment..." -ForegroundColor Cyan
    
    $verifyCommands = @"
docker ps
docker logs --tail 10 ariesv2-api
docker logs --tail 10 ariesv2-ui
"@

    $verifyCommands | ssh -i $sshKeyPath $remoteHost

    Write-Host "`n✅ Deployment update completed successfully!" -ForegroundColor Green
    Write-Host "Please verify the application is working correctly at:"
    Write-Host "UI: http://35.175.254.171:8080" -ForegroundColor Yellow
    Write-Host "API: http://35.175.254.171:5000" -ForegroundColor Yellow
}
catch {
    Write-Host "`n❌ Error: $_" -ForegroundColor Red
    Write-Host "`nTo rollback, connect to the server and run:" -ForegroundColor Yellow
    Write-Host "ssh -i $sshKeyPath $remoteHost"
    Write-Host "cd $remoteDir"
    Write-Host "docker compose down"
    Write-Host "docker image ls  # Find previous image IDs"
    Write-Host "docker tag <previous-api-id> kenaguilar7/ariesv2-api:latest"
    Write-Host "docker tag <previous-ui-id> kenaguilar7/ariesv2-ui:latest"
    Write-Host "docker compose up -d"
    exit 1
} 