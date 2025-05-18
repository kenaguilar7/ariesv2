#!/bin/bash

# Set environment variables for development
export ASPNETCORE_ENVIRONMENT="Development"

# Start the API in the background
cd ./src/Services/Aries.WebAPI
dotnet run &

# Wait for API to start
sleep 5

# Start the UI in debug mode
cd ../UI/Aries.Contabilidad
dotnet run 