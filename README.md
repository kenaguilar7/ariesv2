# Aries Contabilidad 2.0

A modern accounting software solution built with .NET, designed to help businesses manage their financial records efficiently.

## Project Structure

This project follows a clean architecture approach with clear separation of concerns. See [solution-structure.md](solution-structure.md) for detailed organization.

## Prerequisites

- Windows 10 or later
- .NET 7.0 SDK or later
- Visual Studio 2022 or later
- SQL Server (LocalDB or higher)
- Git

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/kenaguilar7/aries-desktop.git
   cd aries-desktop
   ```

2. Set up configuration:
   - Navigate to the `config` directory
   - Copy `appsettings.json.example` to `src/UI/Aries.Contabilidad/appsettings.json`
   - Update the configuration values as needed

3. Build the solution:
   ```bash
   dotnet restore
   dotnet build
   ```

4. Run the application:
   ```bash
   cd src/UI/Aries.Contabilidad
   dotnet run
   ```

## Documentation

Our documentation is hosted on GitHub Pages at [https://kenaguilar7.github.io/aries-desktop/](https://kenaguilar7.github.io/aries-desktop/)

To build the documentation locally:
1. Install DocFX: `dotnet tool install -g docfx`
2. Navigate to the docs folder: `cd docs`
3. Run DocFX: `docfx docfx.json --serve`

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -am 'Add new feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Submit a pull request

## Configuration Management

- Configuration files are not tracked in Git
- Example configurations are provided in the `config` directory
- Copy example files and remove the `.example` extension
- Update values according to your environment

## GitHub Pages Setup

Our documentation is automatically built and deployed using GitHub Actions:
1. Documentation source is in the `docs` folder
2. Built using DocFX
3. Deployed to GitHub Pages on each push to main
4. Configuration in `.github/workflows/pages.yml`

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Original project by [@kenaguilar7](https://github.com/kenaguilar7)
- Contributors and community members 