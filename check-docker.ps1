# Read AWS credentials
$credentialsPath = Join-Path $PSScriptRoot "secrets\aws-cli-user.json"
$credentials = Get-Content $credentialsPath | ConvertFrom-Json

# EC2 instance details
$instanceId = "i-061f6bb6842436017"
$instancePublicIP = "54.163.125.123"

# AWS CLI path
$awsExe = "C:\Program Files\Amazon\AWSCLIV2\aws.exe"

Write-Host "Checking Docker installation on EC2 instance: $instanceId"
Write-Host "Instance IP: $instancePublicIP"

# First try using AWS Systems Manager
Write-Host "`nMethod 1: Checking using AWS Systems Manager..."
try {
    & $awsExe ssm describe-instance-information --filters "Key=InstanceIds,Values=$instanceId" --output json
    & $awsExe ssm send-command `
        --instance-ids $instanceId `
        --document-name "AWS-RunShellScript" `
        --parameters commands=["docker --version"] `
        --output text
} catch {
    Write-Host "Note: AWS Systems Manager not available or not configured on this instance." -ForegroundColor Yellow
}

Write-Host "`nMethod 2: Using AWS EC2 Instance Connect..."
try {
    & $awsExe ec2-instance-connect send-ssh-public-key `
        --instance-id $instanceId `
        --availability-zone us-east-1a `
        --instance-os-user ec2-user

    Write-Host "`nTrying to check Docker version on the instance..."
    & $awsExe ssm start-session `
        --target $instanceId `
        --document-name AWS-StartInteractiveCommand `
        --parameters command="docker --version"
} catch {
    Write-Host "Note: EC2 Instance Connect not available." -ForegroundColor Yellow
}

Write-Host "`nTo check manually via SSH, run the following command:"
Write-Host "ssh -i <path-to-your-key.pem> ec2-user@$instancePublicIP 'docker --version'" 