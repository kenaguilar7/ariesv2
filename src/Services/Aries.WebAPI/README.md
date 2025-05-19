# Aries WebAPI Configuration Guide

## Configuration Files

The application uses different configuration files for different environments:

- `appsettings.json` - Base configuration file with placeholder values
- `appsettings.Development.json` - Development environment configuration
- `appsettings.Example.json` - Example configuration template for reference
- `appsettings.Local.json` - Local development settings (git-ignored)

## Connection Strings Management

1. Local Development:
   - Create `appsettings.Local.json` (this file is git-ignored)
   - Put your local database connection string in this file
   - This file will override the default settings for your local environment

2. Development Environment:
   - Use `appsettings.Development.json` for shared development settings
   - Do not put sensitive credentials in this file

3. Production Environment:
   - Use environment variables or Azure Key Vault/AWS Secrets Manager
   - Set the connection string using:
     ```bash
     # For Windows PowerShell
     $env:ConnectionStrings__MySQL.Aries="your_connection_string"
     
     # For Linux/macOS
     export ConnectionStrings__MySQL.Aries="your_connection_string"
     ```

## Setting Up Local Development

1. Copy `appsettings.Example.json` to `appsettings.Local.json`
2. Update the connection string in `appsettings.Local.json` with your local database credentials
3. Never commit `appsettings.Local.json` to source control

## Production Configuration

For production deployment:

1. Never commit sensitive information to source control
2. Use environment variables or secure configuration management systems
3. Replace the following values with secure production values:
   - MySQL connection string
   - JWT key (use a secure random key with minimum 256 bits length)
   - Any other sensitive configuration

## Security Notes

- Keep production credentials secure and never commit them to source control
- Use different JWT keys for different environments
- Regularly rotate production credentials
- Use secure connection strings in production
- Always use `appsettings.Local.json` for local development credentials 