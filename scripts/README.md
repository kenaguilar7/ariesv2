# Docker Build and Push Scripts

This directory contains scripts to automate the building, pushing, and deployment of Docker images for the Aries Contabilidad project.

## Prerequisites

1. Docker installed and running
2. Docker Hub account
3. Logged in to Docker Hub (`docker login`)
4. AWS Lightsail SSH key at `C:\LightsailDefaultKey-us-east-1.pem`

## Available Scripts

### 1. Build and Push API
```powershell
.\build-and-push-api.ps1
```
This script builds and pushes the API Docker image to Docker Hub.

### 2. Build and Push UI
```powershell
.\build-and-push-ui.ps1
```
This script builds and pushes the UI Docker image to Docker Hub.

### 3. Build and Push All
```powershell
.\build-and-push-all.ps1
```
This script builds and pushes both API and UI Docker images to Docker Hub.

### 4. Publish to AWS (Recommended)
```powershell
.\publish-to-aws.ps1
```
This is the main script for publishing services to AWS. It:
- Builds and pushes both images to Docker Hub
- Updates the deployment on the AWS server
- Verifies the deployment status
- Provides service URLs and troubleshooting steps if needed

## Important Notes

1. Make sure you're logged in to Docker Hub before running these scripts
2. The scripts must be run from PowerShell
3. The AWS SSH key must be present at `C:\LightsailDefaultKey-us-east-1.pem`
4. Services will be available at:
   - UI: http://35.175.254.171:8080
   - API: http://35.175.254.171:5000

## Troubleshooting

If the publish script fails:
1. Check Docker Hub credentials
2. Verify SSH key path
3. Check AWS instance connectivity
4. Review container logs on AWS

For detailed logs, run:
```powershell
ssh -i "C:\LightsailDefaultKey-us-east-1.pem" ubuntu@35.175.254.171 "cd ~/ariesv2 && docker compose logs"
```

## Error Handling

- The scripts include error handling and will display clear error messages
- If any step fails, the script will exit with a non-zero exit code
- Build and push status is clearly indicated with colored output

## Docker Image Tags

- API Image: `kenaguilar7/ariesv2-api:latest`
- UI Image: `kenaguilar7/ariesv2-ui:latest`

## Best Practices

1. Always run `build-and-push-all.ps1` when deploying changes to ensure both images are up to date
2. Check the console output for any errors or warnings
3. Verify the images are pushed successfully by checking Docker Hub 