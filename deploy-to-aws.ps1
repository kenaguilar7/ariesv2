# Save images to tar files
docker save ariesv2-api:latest -o ariesv2-api.tar
docker save ariesv2-ui:latest -o ariesv2-ui.tar

# EC2 instance details
$sshKeyPath = "C:\Users\Steve\Downloads\arie-keypairs.pem"
$instanceIP = "54.163.125.123"
$remoteUser = "ec2-user"
$remotePath = "/home/ec2-user/aries"

# Configure SSH to automatically accept the host key
$env:TERM = "xterm"
Set-Content -Path "$env:USERPROFILE\.ssh\config" -Value @"
Host $instanceIP
    StrictHostKeyChecking no
    UserKnownHostsFile /dev/null
"@ -Force

# Create the remote directory
Write-Host "Creating remote directory..."
ssh -i "$sshKeyPath" "${remoteUser}@${instanceIP}" "mkdir -p $remotePath"

# Copy docker-compose file and images to EC2
Write-Host "Copying files to EC2..."
scp -i "$sshKeyPath" docker-compose.yml "${remoteUser}@${instanceIP}:${remotePath}/"
scp -i "$sshKeyPath" ariesv2-api.tar "${remoteUser}@${instanceIP}:${remotePath}/"
scp -i "$sshKeyPath" ariesv2-ui.tar "${remoteUser}@${instanceIP}:${remotePath}/"

# Deploy containers
Write-Host "Deploying containers..."
$deployCommands = @"
cd $remotePath
docker load -i ariesv2-api.tar
docker load -i ariesv2-ui.tar
docker-compose up -d
"@

ssh -i "$sshKeyPath" "${remoteUser}@${instanceIP}" "$deployCommands"

Write-Host "Deployment completed!" 