# Database Setup Documentation

## Overview
This document describes the local development database setup for Aries Contabilidad 2.0. The database configuration is maintained separately from the main application configuration for better organization and modularity.

## Configuration Files
- `data/docker-compose.db.yml`: Contains MySQL database configuration
- `docker-compose.yml`: Main application configuration (API and UI)

## Configuration Details

### Database Information
- **Database Name**: AriesContabilidad_Local
- **Port**: 3307 (to avoid conflicts with local MySQL installations)
- **Character Set**: utf8mb4
- **Collation**: utf8mb4_unicode_ci

### Access Credentials
- **Username**: aries_user
- **Password**: aries_pwd
- **Root Password**: aries_root_pwd

### Data Persistence
Database data is persisted in the `./data/mysql` directory.

### Connection String
```
Server=localhost;Database=AriesContabilidad_Local;Uid=aries_user;Pwd=aries_pwd;CharSet=utf8mb4;Port=3307
```

## Directory Structure
```
data/
├── docker-compose.db.yml  # Database configuration
└── mysql/                 # Persistent database data
    ├── data/             # Database files
    └── init/             # Initial database scripts
```

## Initial Setup Steps
1. Ensure Docker and Docker Compose are installed
2. Start the database first:
   ```powershell
   cd data
   docker-compose -f docker-compose.db.yml up -d
   cd ..
   ```
3. Wait for the database to initialize (check logs with `docker-compose -f data/docker-compose.db.yml logs mysql`)
4. Start the application services:
   ```powershell
   docker-compose up -d
   ```

## Importing Data
To import existing data:
1. Place SQL dump files in `data/mysql/init/`
2. Files will be executed in alphabetical order during container initialization
3. For manual imports, use the provided import scripts in the `scripts` directory

## Backup and Restore
- Backup scripts are located in `scripts/db-backup.ps1`
- Backup files are stored in `data/mysql/backups/`
- To restore, use the provided restore script in `scripts/db-restore.ps1`

## Troubleshooting
1. If the database fails to start, check the logs:
   ```powershell
   docker-compose -f data/docker-compose.db.yml logs mysql
   ```
2. For permission issues, ensure the data directory has correct permissions
3. If the database is not accessible, verify:
   - Docker container is running (`docker ps`)
   - Port 3307 is not in use
   - Database container is healthy (`docker ps` should show "healthy" status) 