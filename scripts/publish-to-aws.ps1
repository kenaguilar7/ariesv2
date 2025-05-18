# AWS Services Publication Script
$ErrorActionPreference = "Stop"

# Configuration
$sshKeyPath = "C:\LightsailDefaultKey-us-east-1.pem"
$remoteHost = "ubuntu@35.175.254.171"
$remoteDir = "~/ariesv2"

function Write-StepHeader {
    param([string]$message)
    Write-Host "`n🚀 $message" -ForegroundColor Cyan
}

try {
    Write-Host "Starting AWS services publication process..." -ForegroundColor Green

    # Ensure we're in the root directory
    $scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
    $rootPath = Split-Path -Parent $scriptPath
    Set-Location $rootPath

    # Step 1: Build and push Docker images with no cache
    Write-StepHeader "Building and pushing Docker images (with fresh build)..."
    
    # Build API
    Write-Host "Building API image..." -ForegroundColor Yellow
    docker build --no-cache -t kenaguilar7/ariesv2-api:latest -f docker/api/Dockerfile.api .
    if ($LASTEXITCODE -ne 0) { throw "Failed to build API image!" }
    
    # Build UI
    Write-Host "Building UI image..." -ForegroundColor Yellow
    docker build --no-cache -t kenaguilar7/ariesv2-ui:latest -f docker/ui/Dockerfile.ui .
    if ($LASTEXITCODE -ne 0) { throw "Failed to build UI image!" }

    # Push images
    Write-Host "Pushing images to Docker Hub..." -ForegroundColor Yellow
    docker push kenaguilar7/ariesv2-api:latest
    if ($LASTEXITCODE -ne 0) { throw "Failed to push API image!" }
    docker push kenaguilar7/ariesv2-ui:latest
    if ($LASTEXITCODE -ne 0) { throw "Failed to push UI image!" }

    # Step 2: Update remote deployment
    Write-StepHeader "Updating remote deployment..."
    
    # Pull latest images and restart containers with forced pull
    $deployCommands = @"
cd $remoteDir && 
docker compose pull --no-parallel && 
docker compose down && 
docker compose up -d --force-recreate
"@ -replace "`r`n", " "

    # Execute deployment commands
    ssh -i $sshKeyPath $remoteHost $deployCommands
    if ($LASTEXITCODE -ne 0) { throw "Failed to update remote deployment!" }

    # Step 3: Verify deployment
    Write-StepHeader "Verifying deployment..."
    ssh -i $sshKeyPath $remoteHost "docker ps"
    if ($LASTEXITCODE -ne 0) { throw "Failed to verify deployment!" }

    # Step 4: Show container logs
    Write-StepHeader "Showing recent container logs..."
    ssh -i $sshKeyPath $remoteHost "cd $remoteDir && docker compose logs --tail=50"

    Write-Host "`n✅ Services have been successfully published!" -ForegroundColor Green
    Write-Host "Services are available at:"
    Write-Host "UI: http://35.175.254.171:8080" -ForegroundColor Yellow
    Write-Host "API: http://35.175.254.171:5000" -ForegroundColor Yellow

    # Return to original directory
    Set-Location $scriptPath
}
catch {
    Write-Host "`n❌ Error: $_" -ForegroundColor Red
    Write-Host "`nTroubleshooting steps:" -ForegroundColor Yellow
    Write-Host "1. Check Docker Hub credentials"
    Write-Host "2. Verify SSH key path: $sshKeyPath"
    Write-Host "3. Check AWS instance connectivity"
    Write-Host "4. Review container logs on AWS:"
    Write-Host "   ssh -i `"$sshKeyPath`" $remoteHost `"cd $remoteDir && docker compose logs`""

    # Return to original directory even if there's an error
    Set-Location $scriptPath
    exit 1
} 