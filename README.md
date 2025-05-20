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

# Aries Contabilidad 2.0 Database Setup

This guide explains how to set up and manage the MySQL database for Aries Contabilidad 2.0 using Docker.

## Prerequisites

- Docker Desktop installed and running
- PowerShell (Windows) or Terminal (macOS/Linux)
- At least 2GB of free RAM for the database
- At least 1GB of free disk space

## Quick Start

1. Clone the repository:
   ```bash
   git clone https://github.com/kenaguilar7/ariesv2.git
   cd ariesv2/data
   ```

2. Run the deployment script:
   ```powershell
   .\deploy-db.ps1
   ```

The script will:
- Stop any existing database containers
- Clean up old data
- Create necessary directories
- Start a fresh database instance
- Wait for the database to be ready
- Display connection details

## Manual Setup

If you prefer to set up manually:

1. Navigate to the data directory:
   ```bash
   cd data
   ```

2. Create required directories:
   ```bash
   mkdir -p mysql/data mysql/init
   ```

3. Start the database:
   ```bash
   docker-compose -f docker-compose.db.yml up -d
   ```

## Connection Details

- Host: localhost
- Port: 3307
- Database: AriesContabilidad_Local
- Username: aries_user
- Password: aries_pwd

## Common Commands

Stop the database:
```bash
docker-compose -f docker-compose.db.yml down
```

View logs:
```bash
docker-compose -f docker-compose.db.yml logs -f
```

Connect using MySQL client:
```bash
mysql -h localhost -P 3307 -u aries_user -paries_pwd AriesContabilidad_Local
```

## Database Structure

The initialization files in `mysql/init` are executed in alphabetical order:

1. `00-init.sql` - Creates database schema and tables
2. `01-init.sql` - Sets up additional configuration
3. `01-routines.sql` - Creates stored procedures
4. Other `aries_*.sql` files - Load initial data

## Troubleshooting

1. If the database fails to start:
   - Check Docker is running
   - Ensure ports 3307 is not in use
   - Check system has enough memory

2. If you can't connect:
   - Wait 30 seconds for database initialization
   - Verify Docker container is running: `docker ps`
   - Check logs: `docker-compose -f docker-compose.db.yml logs`

## Complete Reset

To completely reset the database:

```bash
docker-compose -f docker-compose.db.yml down -v
rm -rf mysql/data
docker-compose -f docker-compose.db.yml up -d
``` 