# Docker Build and Publish Guide for Aries Contabilidad

## Quick Reference
- **Application**: Aries Contabilidad v2.0
- **Docker Images**: UI (Web), API (Backend)
- **Registry**: Docker Hub
- **Repository**: kenaguilar7/ariesv2
- **Build Tool**: Docker Compose

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Project Structure](#project-structure)
3. [Build Process](#build-process)
4. [Testing Images](#testing-images)
5. [Publishing Images](#publishing-images)
6. [Automation Scripts](#automation-scripts)

## Prerequisites

### Required Tools
- .NET SDK 6.0 or later
- Docker Desktop
- PowerShell 5.0 or later
- Git

### Required Access
- Docker Hub account with write access
- GitHub repository access
- Local development environment

### Environment Setup
```powershell
# Verify development tools
dotnet --version
docker --version
docker compose version
git --version

# Login to Docker Hub
docker login

# Clone repository (if needed)
git clone https://github.com/kenaguilar7/ariesv2.git
cd ariesv2
```

## Project Structure

### Source Code Organization
```
aries.contabilidad.2.0/
├── src/
│   ├── Services/
│   │   └── Aries.WebAPI/          # API Project
│   │       ├── Dockerfile         # API Dockerfile
│   │       └── appsettings.json
│   └── UI/
│       └── Aries.Contabilidad/    # UI Project
│           ├── Dockerfile         # UI Dockerfile
│           └── appsettings.json
├── docker-compose.yml             # Main compose file
└── scripts/
    ├── build-images.ps1          # Build script
    └── publish-images.ps1        # Publish script
```

### Docker Compose Configuration
```yaml
version: '3.8'

services:
  api:
    image: aries-api:latest
    build:
      context: .
      dockerfile: src/Services/Aries.WebAPI/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:5000
    ports:
      - "5000:5000"

  ui:
    image: aries-ui:latest
    build:
      context: .
      dockerfile: src/UI/Aries.Contabilidad/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    ports:
      - "80:80"
    depends_on:
      - api
```

## Build Process

### 1. Clean Build Environment
```powershell
# Remove existing containers
docker compose down

# Clean Docker cache (optional)
docker system prune -f

# Remove existing images
docker rmi aries-ui:latest aries-api:latest
```

### 2. Build Images
```powershell
# Build all images
docker compose build --no-cache

# Build specific images
docker compose build api
docker compose build ui

# View built images
docker images | Select-String "aries-"
```

### 3. Build Arguments (if needed)
```powershell
# Build with specific arguments
docker compose build --build-arg VERSION=1.0.1
```

## Testing Images

### 1. Local Testing
```powershell
# Start services
docker compose up -d

# Check container status
docker ps

# View logs
docker compose logs

# Test endpoints
curl http://localhost:80
curl http://localhost:5000/health
```

### 2. Integration Testing
```powershell
# Run integration tests
dotnet test

# Check API health
curl http://localhost:5000/health

# Monitor logs during testing
docker compose logs -f
```

### 3. Performance Testing
```powershell
# Monitor resource usage
docker stats

# Check container metrics
docker inspect aries-ui-1
docker inspect aries-api-1
```

## Publishing Images

### 1. Tag Images
```powershell
# Tag with version
$VERSION="1.0.0"
docker tag aries-ui:latest kenaguilar7/ariesv2-ui:$VERSION
docker tag aries-api:latest kenaguilar7/ariesv2-api:$VERSION

# Tag latest
docker tag aries-ui:latest kenaguilar7/ariesv2-ui:latest
docker tag aries-api:latest kenaguilar7/ariesv2-api:latest
```

### 2. Push to Registry
```powershell
# Push versioned tags
docker push kenaguilar7/ariesv2-ui:$VERSION
docker push kenaguilar7/ariesv2-api:$VERSION

# Push latest tags
docker push kenaguilar7/ariesv2-ui:latest
docker push kenaguilar7/ariesv2-api:latest
```

### 3. Verify Published Images
```powershell
# List remote tags
docker hub-tool tags kenaguilar7/ariesv2-ui
docker hub-tool tags kenaguilar7/ariesv2-api
```

## Automation Scripts

### Build Script (build-images.ps1)
```powershell
# build-images.ps1
param(
    [string]$Version = "latest",
    [switch]$NoBuildCache
)

# Set error action
$ErrorActionPreference = "Stop"

# Build parameters
$BuildArgs = @()
if ($NoBuildCache) {
    $BuildArgs += "--no-cache"
}

try {
    Write-Host "Building Docker images for Aries Contabilidad v$Version..."
    
    # Build images
    docker compose build $BuildArgs

    # Tag images
    docker tag aries-ui:latest kenaguilar7/ariesv2-ui:$Version
    docker tag aries-api:latest kenaguilar7/ariesv2-api:$Version

    Write-Host "Build completed successfully!"
} catch {
    Write-Error "Build failed: $_"
    exit 1
}
```

### Publish Script (publish-images.ps1)
```powershell
# publish-images.ps1
param(
    [string]$Version = "latest",
    [switch]$Force
)

# Set error action
$ErrorActionPreference = "Stop"

try {
    Write-Host "Publishing Docker images version $Version..."

    # Check Docker Hub login
    docker login

    # Push images
    docker push kenaguilar7/ariesv2-ui:$Version
    docker push kenaguilar7/ariesv2-api:$Version

    if ($Version -ne "latest") {
        # Also update latest tag
        docker tag kenaguilar7/ariesv2-ui:$Version kenaguilar7/ariesv2-ui:latest
        docker tag kenaguilar7/ariesv2-api:$Version kenaguilar7/ariesv2-api:latest
        
        docker push kenaguilar7/ariesv2-ui:latest
        docker push kenaguilar7/ariesv2-api:latest
    }

    Write-Host "Publication completed successfully!"
} catch {
    Write-Error "Publication failed: $_"
    exit 1
}
```

## Best Practices

### 1. Image Building
- Use multi-stage builds to minimize image size
- Include only necessary files
- Set appropriate environment variables
- Use .dockerignore file

### 2. Version Control
- Always tag images with specific versions
- Use semantic versioning
- Keep consistent version numbers across images
- Document version changes

### 3. Security
- Scan images for vulnerabilities
- Use minimal base images
- Don't include sensitive data
- Follow least privilege principle

### 4. CI/CD Integration
- Automate build process
- Include automated testing
- Implement version control
- Use build matrices for different platforms

## Troubleshooting

### Common Build Issues
1. **Build Failures**
```powershell
# Clean Docker cache
docker system prune -a

# Check Docker daemon
docker info
```

2. **Push Failures**
```powershell
# Verify Docker login
docker login

# Check image existence
docker images
```

3. **Resource Issues**
```powershell
# Check system resources
docker system df
docker system info
```

## Support Information

### Documentation
- Docker Documentation: https://docs.docker.com/
- Project Repository: https://github.com/kenaguilar7/ariesv2
- Docker Hub: https://hub.docker.com/r/kenaguilar7/ariesv2

### Logs and Debugging
- Build Logs: Check Docker build output
- Container Logs: `docker compose logs`
- System Logs: `docker system events`

### Contact
For build and publishing issues:
- Development Team: [Insert contact]
- DevOps Support: [Insert contact] 