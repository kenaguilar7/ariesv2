# Remove the solution file
Remove-Item -Path "Aries.sln" -ErrorAction SilentlyContinue

# Create a new solution file
dotnet new sln

# Add all projects in the new structure
Get-ChildItem -Recurse -Path "src" -Filter "*.csproj" | ForEach-Object {
    Write-Host "Adding project: $($_.FullName)"
    dotnet sln add $_.FullName
}

# Update project references
Get-ChildItem -Recurse -Path "src" -Filter "*.csproj" | ForEach-Object {
    $projectPath = $_.FullName
    $projectContent = Get-Content $projectPath -Raw
    
    # Update project references to use relative paths
    $projectContent = $projectContent -replace 'Include="[^"]*Aries.Contabilidad.Core.csproj"', 'Include="..\..\Core\Aries.Contabilidad.Core\Aries.Contabilidad.Core.csproj"'
    $projectContent = $projectContent -replace 'Include="[^"]*AriesContador.Core.csproj"', 'Include="..\..\Core\AriesContador.Core\AriesContador.Core.csproj"'
    $projectContent = $projectContent -replace 'Include="[^"]*AriesContador.Data.csproj"', 'Include="..\..\Core\AriesContador.Data\AriesContador.Data.csproj"'
    
    # Save the updated content
    $projectContent | Set-Content $projectPath -NoNewline
}

Write-Host "Solution and project references have been updated" 