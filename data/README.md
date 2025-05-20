# Aries Database Setup

This directory contains all database-related configurations and data for the Aries Contabilidad project.

## Directory Structure

```
data/
├── docker-compose.db.yml    # Database container configuration
├── mysql/
│   ├── data/               # Persistent MySQL data
│   └── init/              # Initialization SQL scripts
```

## Connection Details

- **Host**: localhost
- **Port**: 3307
- **Database**: AriesContabilidad_Local
- **Username**: aries_user
- **Password**: aries_pwd

### Connection String
```
Server=localhost;Port=3307;Database=AriesContabilidad_Local;Uid=aries_user;Pwd=aries_pwd;
```

## Usage

1. Start the database:
   ```bash
   docker-compose -f docker-compose.db.yml up -d
   ```

2. Stop the database:
   ```bash
   docker-compose -f docker-compose.db.yml down
   ```

3. View logs:
   ```bash
   docker-compose -f docker-compose.db.yml logs -f
   ```

## Data Persistence

The MySQL data is persisted in the `./mysql/data` directory. This ensures your data survives container restarts. 