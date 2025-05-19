# AWS Deployment Guide for Aries Contabilidad

## Quick Reference
- **Application**: Aries Contabilidad v2.0
- **Platform**: AWS Lightsail
- **Base OS**: Ubuntu
- **Components**: UI (Web), API (Backend)
- **Default Ports**: UI (80), API (5000)

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Initial Server Setup](#initial-server-setup)
3. [Application Deployment](#application-deployment)
4. [Update Procedure](#update-procedure)
5. [Maintenance Guide](#maintenance-guide)
6. [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Files
- AWS Lightsail instance running Ubuntu
- SSH key file: `aries-dev-server.pem`
- Docker images:
  - `aries-ui.tar`
  - `aries-api.tar`
- Configuration: `docker-compose.yml`

### Local Development Structure
```
aries.contabilidad.2.0/
├── src/
│   ├── Services/
│   │   └── Aries.WebAPI/    # API Project
│   └── UI/
│       └── Aries.Contabilidad/    # UI Project
├── scripts/
│   ├── aws-deployment-guide.md
│   ├── publish-to-aws.ps1         # Deployment script
│   └── update-aws-deployment.ps1  # Update script
└── docker-compose.yml
```

### Server Structure
```
/home/ubuntu/
└── ariesv2/                 # Main application directory
    ├── docker-compose.yml   # Container configuration
    ├── aries-ui.tar        # UI Docker image
    ├── aries-api.tar       # API Docker image
    └── backups/            # Backup directory
        ├── docker-compose.yml.backup
        ├── aries-ui-backup.tar
        └── aries-api-backup.tar
```

## Initial Server Setup

### 1. SSH Connection
```bash
# Replace YOUR_INSTANCE_IP with your AWS Lightsail instance IP
ssh -i "C:\aries-dev-server.pem" ubuntu@54.80.155.87
```

### 2. Install Docker
```bash
# First, remove any old versions and update the system
sudo apt-get remove docker docker-engine docker.io containerd runc
sudo apt-get update
sudo apt-get upgrade -y

# Install required packages
sudo apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

# Add Docker's official GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Install Docker and Docker Compose
sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io

# Install Docker Compose (binary method - more reliable)
sudo curl -L "https://github.com/docker/compose/releases/download/v2.23.3/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Add user to docker group
sudo usermod -aG docker $USER

# Apply group changes (either log out and back in, or run this command)
newgrp docker

# Verify installations
docker --version
docker-compose --version

# Test Docker installation
docker run hello-world

# Note: If you encounter dependency issues with docker-compose-v2 package, 
# use the binary installation method shown above instead.
```

### 3. Configure Firewall
```bash
# Enable UFW
sudo ufw enable

# Allow SSH (important!)
sudo ufw allow ssh

# Allow application ports
sudo ufw allow 80/tcp
sudo ufw allow 5000/tcp

# Verify rules
sudo ufw status
```

## Application Deployment

### Method 1: Using Docker Hub (Recommended)

First, create the proper production docker-compose.yml:

```yaml
version: '3.8'

services:
  api:
    image: kenaguilar7/ariesv2-api:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:5000
    ports:
      - "5000:5000"
    restart: unless-stopped

  ui:
    image: kenaguilar7/ariesv2-ui:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    ports:
      - "80:80"
    depends_on:
      - api
    restart: unless-stopped

networks:
  default:
    driver: bridge
```

Then proceed with deployment:

```bash
# Create deployment directory
mkdir -p ~/ariesv2

# Create and edit docker-compose.yml with the content above
nano ~/ariesv2/docker-compose.yml

# Login to Docker Hub on the server
docker login

# Pull the images
docker pull kenaguilar7/ariesv2-ui:latest
docker pull kenaguilar7/ariesv2-api:latest

# Start services
cd ~/ariesv2
docker compose up -d

# Verify deployment
docker ps
curl localhost:80  # Test UI
curl localhost:5000  # Test API
```

### Method 2: Using .tar Files (Offline Deployment)

#### 1. Create .tar Files Locally
```powershell
# From your local development machine
cd C:\Users\Steve\aries.contabilidad.2.0

# Save Docker images as .tar files
docker save kenaguilar7/ariesv2-ui:latest -o aries-ui.tar
docker save kenaguilar7/ariesv2-api:latest -o aries-api.tar
```

#### 2. Transfer Files to Server
```powershell
# PowerShell commands (run from your local machine)
$PEM_PATH = "C:\aries-dev-server.pem"
$REMOTE_USER = "ubuntu"
$REMOTE_IP = "54.144.10.65"
$REMOTE_DIR = "~/ariesv2"

# First, ensure the remote directory exists
ssh -i $PEM_PATH ${REMOTE_USER}@${REMOTE_IP} "mkdir -p ${REMOTE_DIR}"

# Transfer files (make sure you're in the directory containing the files)
scp -i $PEM_PATH docker-compose.yml ${REMOTE_USER}@${REMOTE_IP}:${REMOTE_DIR}/
scp -i $PEM_PATH ./aries-ui.tar ${REMOTE_USER}@${REMOTE_IP}:${REMOTE_DIR}/
scp -i $PEM_PATH ./aries-api.tar ${REMOTE_USER}@${REMOTE_IP}:${REMOTE_DIR}/
```

#### 3. Load Docker Images
```bash
# On the server
cd ~/ariesv2
docker load -i aries-ui.tar
docker load -i aries-api.tar
```

#### 4. Deploy Application
```bash
# Start services
docker compose up -d

# Verify deployment
docker ps
curl localhost:80  # Test UI
curl localhost:5000  # Test API
```

## Update Procedure

### 1. Backup Current State
```bash
# Stop services
cd ~/ariesv2
docker compose down

# Backup files
cp docker-compose.yml backups/docker-compose.yml.$(date +%Y%m%d)
docker save aries-ui:latest > backups/aries-ui.$(date +%Y%m%d).tar
docker save aries-api:latest > backups/aries-api.$(date +%Y%m%d).tar
```

### 2. Deploy Updates
```bash
# Load new images
docker load -i aries-ui.tar
docker load -i aries-api.tar

# Start services
docker compose up -d

# Verify update
docker ps
curl localhost:80
curl localhost:5000
```

### 3. Rollback (if needed)
```bash
# Stop services
docker compose down

# Restore from backup
cp backups/docker-compose.yml.backup docker-compose.yml
docker load -i backups/aries-ui-backup.tar
docker load -i backups/aries-api-backup.tar

# Restart services
docker compose up -d
```

## Maintenance Guide

### Regular Maintenance Tasks
```bash
# Update system packages
sudo apt update && sudo apt upgrade -y

# Check disk space
df -h

# View Docker space usage
docker system df

# Clean up unused resources
docker system prune -a --volumes

# Remove old backups (older than 3 days)
find ~/ariesv2/backups/ -name "*.tar" -mtime +3 -delete
find ~/ariesv2/backups/ -name "docker-compose.yml.*" -mtime +3 -delete
```

### Monitoring Commands
```bash
# View running containers
docker ps

# Check container logs
docker logs aries-ui-1
docker logs aries-api-1

# Monitor resource usage
docker stats

# View application logs
docker compose logs
```

## Troubleshooting

### Common Issues and Solutions

1. **Container Won't Start**
```bash
# Check logs
docker logs aries-ui-1
docker logs aries-api-1

# Verify image existence
docker images | grep aries
```

2. **Port Conflicts**
```bash
# Check used ports
sudo netstat -tuln

# Verify firewall rules
sudo ufw status
```

3. **Permission Issues**
```bash
# Fix directory permissions
sudo chown -R ubuntu:ubuntu ~/ariesv2
chmod 755 ~/ariesv2
```

4. **Disk Space Issues**
```bash
# Clean up Docker system
docker system prune -a

# Clean old backups
find ~/ariesv2/backups/ -mtime +7 -delete
```

### Security Notes
1. Keep the PEM file secure and never share it
2. Use environment variables for sensitive data
3. Regularly update system packages
4. Monitor system logs for suspicious activity
5. Maintain regular backups
6. Consider implementing HTTPS in production

### Support Information
- Original Project: https://github.com/kenaguilar7/aries-desktop
- Current Repository: https://github.com/kenaguilar7/ariesv2
- Documentation: Refer to project wiki for detailed information