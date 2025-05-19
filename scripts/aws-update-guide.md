# AWS Update Guide for Aries Contabilidad

## Quick Reference
- **Application**: Aries Contabilidad v2.0
- **Update Type**: Docker Image Update
- **Components**: UI (Web), API (Backend)
- **Default Ports**: UI (80), API (5000)
- **Repository**: kenaguilar7/ariesv2

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Pre-Update Checklist](#pre-update-checklist)
3. [Update Process](#update-process)
4. [Verification](#verification)
5. [Rollback Procedure](#rollback-procedure)
6. [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Tools
- PowerShell 5.0 or later
- Docker Desktop
- AWS Lightsail SSH key
- Access to AWS instance

### Required Access
- Docker Hub account with push access
- AWS Lightsail instance credentials
- SSH key at `C:\LightsailDefaultKey-us-east-1.pem`

### Environment Check
```powershell
# Verify local tools
docker --version
docker compose version
ssh -V

# Check Docker Hub login
docker login

# Test AWS connection
ssh -i "C:\aries-dev-server.pem" ubuntu@54.80.155.87 "echo 'Connection successful'"
```

## Pre-Update Checklist

### 1. Backup Current State
```bash
# On AWS server
cd ~/ariesv2

# Backup docker-compose file
cp docker-compose.yml docker-compose.yml.$(date +%Y%m%d)

# Save current images
docker save aries-ui:latest > backups/aries-ui.$(date +%Y%m%d).tar
docker save aries-api:latest > backups/aries-api.$(date +%Y%m%d).tar
```

### 2. Check System Resources
```bash
# Check disk space
df -h

# Check Docker disk usage
docker system df

# View running containers
docker ps

# Check system load
top -n 1
```

### 3. Verify Current State
```bash
# Test current endpoints
curl -I localhost:80
curl -I localhost:5000/health

# Check current logs for issues
docker compose logs --tail=100
```

## Update Process

### 1. Local Development
```powershell
# Navigate to project root
cd aries.contabilidad.2.0

# Build new images
docker compose build --no-cache

# Test locally
docker compose up -d
curl localhost:8080
curl localhost:5000
```

### 2. Push Updates
```powershell
# Tag images for production
docker tag aries-ui:latest kenaguilar7/ariesv2-ui:latest
docker tag aries-api:latest kenaguilar7/ariesv2-api:latest

# Push to Docker Hub
docker push kenaguilar7/ariesv2-ui:latest
docker push kenaguilar7/ariesv2-api:latest
```

### 3. Server Update
```bash
# Connect to server
ssh -i "C:\aries-dev-server.pem" ubuntu@54.144.10.65

# Navigate to app directory
cd ~/ariesv2

# Stop current services
docker compose down

# Pull new images
docker compose pull

# Start updated services
docker compose up -d
```

## Verification

### 1. Check Services
```bash
# View running containers
docker ps

# Check container health
docker inspect --format='{{.State.Health.Status}}' aries-ui-1
docker inspect --format='{{.State.Health.Status}}' aries-api-1
```

### 2. Monitor Logs
```bash
# View startup logs
docker compose logs --tail=100

# Monitor real-time logs
docker compose logs -f

# Check specific service logs
docker logs aries-ui-1
docker logs aries-api-1
```

### 3. Test Functionality
```bash
# Check endpoints
curl -I localhost:80
curl -I localhost:5000/health

# Monitor resource usage
docker stats
```

## Rollback Procedure

### 1. Stop Current Services
```bash
cd ~/ariesv2
docker compose down
```

### 2. Restore Previous Version
```bash
# Restore docker-compose file
cp backups/docker-compose.yml.backup docker-compose.yml

# Load backup images
docker load -i backups/aries-ui-backup.tar
docker load -i backups/aries-api-backup.tar

# Start previous version
docker compose up -d
```

### 3. Verify Rollback
```bash
# Check services
docker ps

# Test endpoints
curl -I localhost:80
curl -I localhost:5000/health
```

## Troubleshooting

### Common Issues

1. **Image Pull Failures**
```bash
# Check Docker Hub connectivity
curl -v https://hub.docker.com

# Verify Docker login
docker login

# Clear Docker cache
docker system prune -a
```

2. **Container Start Failures**
```bash
# Check detailed logs
docker logs aries-ui-1 --tail 50
docker logs aries-api-1 --tail 50

# Verify port availability
sudo netstat -tulpn | grep -E '80|5000'
```

3. **Performance Issues**
```bash
# Check resource usage
docker stats

# Monitor system resources
top -n 1

# Check disk space
df -h
```

### Maintenance Commands

```bash
# Clean up old images
docker image prune -a

# Remove unused volumes
docker volume prune

# Clean old backups (older than 7 days)
find ~/ariesv2/backups/ -mtime +7 -delete
```

## Security Best Practices

1. **Image Security**
   - Always pull from trusted sources
   - Use specific image tags, not 'latest'
   - Regularly scan for vulnerabilities

2. **Access Control**
   - Use least privilege principle
   - Rotate access tokens regularly
   - Monitor access logs

3. **Data Protection**
   - Maintain regular backups
   - Encrypt sensitive data
   - Use secure connections

## Support Information

### Documentation
- Original Project: https://github.com/kenaguilar7/aries-desktop
- Current Repository: https://github.com/kenaguilar7/ariesv2
- Docker Hub: https://hub.docker.com/r/kenaguilar7/ariesv2

### Logs Location
- Application Logs: `docker compose logs`
- System Logs: `/var/log/syslog`
- Docker Logs: `journalctl -u docker`

### Contact
For urgent issues or assistance:
- Technical Support: [Insert contact]
- Emergency Contact: [Insert contact] 