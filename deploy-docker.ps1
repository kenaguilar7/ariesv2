# Read AWS credentials
$credentialsPath = Join-Path $PSScriptRoot "secrets\aws-cli-user.json"
$credentials = Get-Content $credentialsPath | ConvertFrom-Json

# EC2 instance details
$instanceId = "i-061f6bb6842436017"
$instancePublicIP = "54.163.125.123"

# AWS CLI path
$awsExe = "C:\Program Files\Amazon\AWSCLIV2\aws.exe"

Write-Host "Starting Docker deployment to EC2 instance: $instanceId"
Write-Host "Instance IP: $instancePublicIP"

# Function to check if a command exists
function Test-Command($cmdname) {
    return $null -ne (Get-Command -Name $cmdname -ErrorAction SilentlyContinue)
}

# Check prerequisites
$prerequisites = @(
    @{Name = "AWS CLI"; Test = { Test-Path $awsExe }},
    @{Name = "Docker"; Test = { Test-Command "docker" }},
    @{Name = "SSH"; Test = { Test-Command "ssh" }}
)

$missingPrereqs = $false
foreach ($prereq in $prerequisites) {
    if (-not (& $prereq.Test)) {
        Write-Host "❌ Missing prerequisite: $($prereq.Name)" -ForegroundColor Red
        $missingPrereqs = $true
    } else {
        Write-Host "✅ Found $($prereq.Name)" -ForegroundColor Green
    }
}

if ($missingPrereqs) {
    Write-Host "`nPlease install missing prerequisites and try again." -ForegroundColor Red
    exit 1
}

# Check EC2 instance status
Write-Host "`nChecking EC2 instance status..."
try {
    $instanceStatus = & $awsExe ec2 describe-instance-status --instance-ids $instanceId --query 'InstanceStatuses[0].InstanceStatus.Status' --output text
    if ($instanceStatus -ne "ok") {
        Write-Host "EC2 instance is not running properly. Status: $instanceStatus" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ EC2 instance is running" -ForegroundColor Green
} catch {
    Write-Host "Error checking EC2 status: $_" -ForegroundColor Red
    exit 1
}

Write-Host "`n📋 Deployment Instructions:"
Write-Host "1. Make sure you have your EC2 key pair file (.pem)"
Write-Host "2. Ensure Docker image is built and ready"
Write-Host "3. Run the following commands manually (replace placeholders):"
Write-Host "`n# Test SSH connection:"
Write-Host "ssh -i <path-to-your-key.pem> ec2-user@$instancePublicIP"
Write-Host "`n# Copy Docker image to EC2:"
Write-Host "docker save <your-image-name> | ssh -i <path-to-your-key.pem> ec2-user@$instancePublicIP 'docker load'"
Write-Host "`n# Run the container on EC2:"
Write-Host "ssh -i <path-to-your-key.pem> ec2-user@$instancePublicIP 'docker run -d <your-image-name>'"

Write-Host "`n⚠️ Please provide the following information to proceed:"
Write-Host "1. Path to your EC2 key pair file (.pem)"
Write-Host "2. Name of your Docker image"
Write-Host "3. Any specific container run parameters (ports, environment variables, etc.)" 