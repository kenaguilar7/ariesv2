# Build and Push UI Script
$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting UI build and push process..." -ForegroundColor Green

try {
    # Navigate to root directory
    $scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
    $rootPath = Split-Path -Parent $scriptPath
    Set-Location $rootPath

    # Build the UI image
    Write-Host "🔨 Building UI Docker image..." -ForegroundColor Yellow
    docker build -t kenaguilar7/ariesv2-ui:latest -f docker/ui/Dockerfile.ui .

    # Check if build was successful
    if ($LASTEXITCODE -ne 0) {
        throw "Docker build failed!"
    }

    # Push the image
    Write-Host "📤 Pushing UI image to Docker Hub..." -ForegroundColor Yellow
    docker push kenaguilar7/ariesv2-ui:latest

    if ($LASTEXITCODE -ne 0) {
        throw "Docker push failed!"
    }

    Write-Host "✅ UI build and push completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    exit 1
} 