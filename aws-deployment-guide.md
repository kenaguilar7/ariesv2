# AWS Deployment Guide for Aries Contabilidad

This guide documents all the steps required to deploy the Aries Contabilidad application to AWS Lightsail.

## Prerequisites

1. AWS Lightsail instance running Ubuntu
2. Lightsail SSH key (LightsailDefaultKey-us-east-1.pem)
3. Docker images:
   - ariesv2-ui.tar
   - ariesv2-api.tar

## Step 1: SSH Connection

```bash
# Connect to the server using the PEM file
ssh -i "C:\LightsailDefaultKey-us-east-1.pem" ubuntu@35.175.254.171
```

## Step 2: Install Required Software

```bash
# Update package list
sudo apt update

# Install Docker Compose V2
sudo apt install -y docker-compose-v2

# Add current user to docker group
sudo usermod -aG docker ubuntu
```

## Step 3: Create Application Directory

```bash
# Create and navigate to application directory
mkdir -p ~/ariesv2
cd ~/ariesv2
```

## Step 4: Transfer Files

```bash
# From your local machine, copy the required files
scp -i "C:\LightsailDefaultKey-us-east-1.pem" docker-compose.yml ubuntu@35.175.254.171:~/ariesv2/
scp -i "C:\LightsailDefaultKey-us-east-1.pem" ariesv2-ui.tar ubuntu@35.175.254.171:~/ariesv2/
scp -i "C:\LightsailDefaultKey-us-east-1.pem" ariesv2-api.tar ubuntu@35.175.254.171:~/ariesv2/
```

## Step 5: Load Docker Images

```bash
# Load the Docker images
cd ~/ariesv2
sudo docker load -i ariesv2-ui.tar
sudo docker load -i ariesv2-api.tar
```

## Step 6: Docker Compose Configuration

Create `docker-compose.yml` with the following content:

```yaml
version: '3.8'

services:
  ui:
    image: ariesv2-ui:latest
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      - api
    restart: unless-stopped

  api:
    image: ariesv2-api:latest
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:80
    restart: unless-stopped
```

## Step 7: Start the Services

```bash
# Start the services using Docker Compose
cd ~/ariesv2
sudo docker compose up -d
```

## Step 8: Configure Firewalls

### Server Firewall (UFW)

```bash
# Allow required ports
sudo ufw allow 8080/tcp
sudo ufw allow 5000/tcp
```

### AWS Lightsail Firewall

1. Go to AWS Lightsail Console
2. Select your instance
3. Go to the "Networking" tab
4. Add the following rules:
   - Custom TCP port 8080
   - Custom TCP port 5000

## Step 9: Verify Deployment

```bash
# Check if containers are running
sudo docker ps

# Test local access
curl localhost:8080
curl localhost:5000
```

## Access Points

- UI Application: `http://35.175.254.171:8080`
- API Endpoint: `http://35.175.254.171:5000`

## Useful Commands

### Container Management
```bash
# Stop containers
sudo docker compose down

# View container logs
sudo docker compose logs

# Restart containers
sudo docker compose restart
```

### Troubleshooting
```bash
# View container status
sudo docker ps

# View container logs
sudo docker logs ariesv2-ui-1
sudo docker logs ariesv2-api-1

# Check ports in use
sudo netstat -tuln
```

## Security Notes

1. The PEM file should be kept secure and not shared
2. Consider using environment variables for sensitive data
3. In production, consider using HTTPS with SSL certificates
4. Review and adjust the firewall rules based on your security requirements

## Maintenance

### Updates
```bash
# Pull latest images (if using remote registry)
sudo docker compose pull

# Restart services with new images
sudo docker compose up -d
```

### Backup
```bash
# Save current images
sudo docker save ariesv2-ui:latest > ariesv2-ui-backup.tar
sudo docker save ariesv2-api:latest > ariesv2-api-backup.tar
``` 