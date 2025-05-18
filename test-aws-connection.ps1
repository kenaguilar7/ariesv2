# Read and parse the AWS credentials JSON file
$credentialsPath = Join-Path $PSScriptRoot "secrets\aws-cli-user.json"
$credentials = Get-Content $credentialsPath | ConvertFrom-Json

# Set AWS credentials as environment variables
$env:AWS_ACCESS_KEY_ID = $credentials.aws_access_key_id
$env:AWS_SECRET_ACCESS_KEY = $credentials.aws_secret_access_key
$env:AWS_DEFAULT_REGION = $credentials.region

# Define AWS CLI path
$awsExe = "C:\Program Files\Amazon\AWSCLIV2\aws.exe"

if (-not (Test-Path $awsExe)) {
    Write-Host "Error: AWS CLI not found at $awsExe" -ForegroundColor Red
    Write-Host "Please ensure AWS CLI is installed and provide the correct path."
    exit 1
}

Write-Host "Setting up AWS credentials..."
Write-Host "Region: $($credentials.region)"

# Check AWS CLI version
Write-Host "`nChecking AWS CLI version..."
& $awsExe --version

# Test AWS connection
Write-Host "`nTesting AWS connection..."
try {
    Write-Host "`nGetting caller identity..."
    & $awsExe sts get-caller-identity
    Write-Host "`nConnection successful! Above is your AWS account information."
    
    Write-Host "`nListing EC2 instances..."
    Write-Host "------------------------"
    # Get EC2 instances with detailed information
    & $awsExe ec2 describe-instances --query 'Reservations[*].Instances[*].{
        InstanceId:InstanceId,
        Name:Tags[?Key==`Name`].Value | [0],
        State:State.Name,
        InstanceType:InstanceType,
        PublicIP:PublicIpAddress,
        PrivateIP:PrivateIpAddress
    }' --output table

} catch {
    Write-Host "Error connecting to AWS: $_" -ForegroundColor Red
} 