# AWS Image Update Guide for Aries Contabilidad

This guide documents the process of updating the Docker images for the Aries Contabilidad application in AWS.

## Prerequisites

1. PowerShell installed
2. Docker installed and running
3. Docker Hub account and logged in
4. AWS Lightsail SSH key at `C:\LightsailDefaultKey-us-east-1.pem`
5. Access to the AWS instance

## Step 1: Update and Push Docker Images

### Local Machine Steps

```powershell
# Navigate to the scripts directory
cd scripts

# Run the build and push script
.\build-and-push-all.ps1

# Verify images are pushed to Docker Hub
docker images | findstr kenaguilar7/ariesv2
```

## Step 2: Connect to AWS Instance

```powershell
# Connect using Lightsail SSH key
ssh -i "C:\LightsailDefaultKey-us-east-1.pem" ubuntu@35.175.254.171
```

## Step 3: Update Images on AWS

```bash
# Navigate to application directory
cd ~/ariesv2

# Pull the latest images
docker pull kenaguilar7/ariesv2-api:latest
docker pull kenaguilar7/ariesv2-ui:latest

# Stop the current containers
docker compose down

# Start with new images
docker compose up -d
```

## Step 4: Verify Deployment

```bash
# Check if containers are running
docker ps

# Check container logs
docker logs ariesv2-api
docker logs ariesv2-ui

# Test endpoints
curl localhost:5000/health
curl localhost:8080
```

## Rollback Procedure

If the update causes issues, you can rollback to the previous version:

```bash
# Stop the current containers
docker compose down

# Find the previous image IDs
docker images

# Tag the previous images
docker tag <previous-api-image-id> kenaguilar7/ariesv2-api:latest
docker tag <previous-ui-image-id> kenaguilar7/ariesv2-ui:latest

# Start the containers again
docker compose up -d
```

## Common Issues and Solutions

### 1. Container Won't Start
```bash
# Check detailed logs
docker logs ariesv2-api --tail 50
docker logs ariesv2-ui --tail 50
```

### 2. Network Issues
```bash
# Check if ports are correctly mapped
docker compose ps

# Verify network connectivity
curl localhost:5000
curl localhost:8080
```

### 3. Disk Space Issues
```bash
# Check available disk space
df -h

# Clean up unused Docker resources
docker system prune -f
```

## Monitoring Update Progress

1. Watch container logs in real-time:
```bash
docker logs -f ariesv2-api
docker logs -f ariesv2-ui
```

2. Monitor system resources:
```bash
docker stats
```

## Post-Update Checklist

- [ ] Both containers are running (`docker ps`)
- [ ] API endpoints are responding
- [ ] UI is accessible and functional
- [ ] No error messages in logs
- [ ] Database connections are working
- [ ] Authentication is working

## Security Notes

1. Always verify image integrity
2. Keep SSH keys secure
3. Monitor system logs for unauthorized access attempts
4. Maintain regular backups
5. Follow the principle of least privilege

## Maintenance Tips

1. Regular cleanup:
```bash
# Remove unused images and containers
docker system prune -a

# Clean up old volumes
docker volume prune
```

2. Log rotation:
```bash
# Check log sizes
du -h /var/lib/docker/containers/*/*.log
```

3. Backup important data:
```bash
# Backup configuration
cp docker-compose.yml docker-compose.yml.backup
```

## Contact and Support

For issues or assistance:
- Technical Support: [support@email.com]
- Emergency Contact: [emergency@email.com]
- Documentation: [link-to-documentation] 