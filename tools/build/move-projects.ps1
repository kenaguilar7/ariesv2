# Create necessary directories if they don't exist
New-Item -ItemType Directory -Force -Path "src/Core"
New-Item -ItemType Directory -Force -Path "src/Services"
New-Item -ItemType Directory -Force -Path "src/UI"

# Move Core projects
Move-Item -Path "Aries.Contabilidad.Core" -Destination "src/Core/" -Force
Move-Item -Path "AriesContador.Core" -Destination "src/Core/" -Force
Move-Item -Path "AriesContador.Data" -Destination "src/Core/" -Force

# Move Service projects
Move-Item -Path "Aries.WebAPI" -Destination "src/Services/" -Force
Move-Item -Path "Aries.WebServices" -Destination "src/Services/" -Force

# Move UI project
Move-Item -Path "Aries.Contabilidad" -Destination "src/UI/" -Force

Write-Host "Projects have been moved to their new locations" 