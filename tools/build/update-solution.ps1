# Remove existing projects
dotnet sln Aries.sln remove (Get-ChildItem -Recurse -Filter "*.csproj")

# Add projects from new locations
Get-ChildItem -Recurse -Filter "*.csproj" | ForEach-Object {
    Write-Host "Adding project: $($_.FullName)"
    dotnet sln Aries.sln add $_.FullName
}

Write-Host "Solution file has been updated with new project locations" 