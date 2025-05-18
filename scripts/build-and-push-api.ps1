# Build and Push API Script
$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting API build and push process..." -ForegroundColor Green

try {
    # Navigate to root directory
    $scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
    $rootPath = Split-Path -Parent $scriptPath
    Set-Location $rootPath

    # Build the API image
    Write-Host "🔨 Building API Docker image..." -ForegroundColor Yellow
    docker build -t kenaguilar7/ariesv2-api:latest -f docker/api/Dockerfile.api .

    # Check if build was successful
    if ($LASTEXITCODE -ne 0) {
        throw "Docker build failed!"
    }

    # Push the image
    Write-Host "📤 Pushing API image to Docker Hub..." -ForegroundColor Yellow
    docker push kenaguilar7/ariesv2-api:latest

    if ($LASTEXITCODE -ne 0) {
        throw "Docker push failed!"
    }

    Write-Host "✅ API build and push completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    exit 1
} 