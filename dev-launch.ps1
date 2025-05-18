# Set environment variables for development
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Start the API in a new window
Start-Process powershell -ArgumentList "cd ./src/Services/Aries.WebAPI; dotnet run"

# Wait for API to start
Start-Sleep -Seconds 5

# Start the UI in debug mode
Set-Location ./src/UI/Aries.Contabilidad
dotnet run 