# Build and Push All Images Script
$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting build and push process for all images..." -ForegroundColor Green

try {
    # Get the directory of this script
    $scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path

    # Run API build and push
    Write-Host "`n📦 Processing API image..." -ForegroundColor Cyan
    & "$scriptPath\build-and-push-api.ps1"
    if ($LASTEXITCODE -ne 0) { throw "API build and push failed!" }

    # Run UI build and push
    Write-Host "`n📦 Processing UI image..." -ForegroundColor Cyan
    & "$scriptPath\build-and-push-ui.ps1"
    if ($LASTEXITCODE -ne 0) { throw "UI build and push failed!" }

    Write-Host "`n✅ All images have been built and pushed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "`n❌ Error: $_" -ForegroundColor Red
    exit 1
} 