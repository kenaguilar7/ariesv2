# MySQL Database Management Guide
## Remote Database Access and Cloning

### Current Remote Database Details
- Host: 3.140.15.116
- Port: 3306
- Production Database: AriesContabilidad

### Prerequisites
1. MySQL Workbench (already installed)
2. Access credentials for the remote database
3. Sufficient privileges to create new databases

### Connecting to Remote Database

1. Open MySQL Workbench
2. Create a new connection:
   - Click the '+' icon next to 'MySQL Connections'
   - Set Connection Name: `Aries-Production`
   - Hostname: `3.140.15.116`
   - Port: `3306`
   - Username: Your database username
   - Click 'Store in Vault' and enter your password
   - Test the connection before saving

### Creating Test Database (Clone)

1. **Create New Database**
   ```sql
   CREATE DATABASE IF NOT EXISTS AriesContabilidad_Test
   CHARACTER SET utf8mb4
   COLLATE utf8mb4_unicode_ci;
   ```

2. **Clone Database Structure**
   ```sql
   -- Make sure you're in the new database
   USE AriesContabilidad_Test;

   -- Clone all tables and stored procedures
   CALL mysql.rds_copy_db_to_db('AriesContabilidad', 'AriesContabilidad_Test');
   ```

   Alternative manual method if rds_copy_db_to_db is not available:
   ```sql
   -- Create exact copy of schema
   & "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe" -h ariescontrol.cn28u0mqcci2.us-east-2.rds.amazonaws.com -u kenneth -p --no-data aries_contabilidad_dev > aries_backup.sql
   mysql -h 3.140.15.116 -u your_username -p AriesContabilidad_Test < aries_schema.sql
   ```

3. **Clone Data (Optional)**
   ```sql
   -- If you want to copy all data
   INSERT INTO AriesContabilidad_Test.table_name
   SELECT * FROM AriesContabilidad.table_name;

   -- If you want to copy specific data
   INSERT INTO AriesContabilidad_Test.users
   SELECT * FROM AriesContabilidad.users
   WHERE created_at >= DATE_SUB(NOW(), INTERVAL 30 DAY);
   ```

### Updating Application Configuration

1. Create or update `appsettings.Local.json`:
   ```json
   {
     "ConnectionStrings": {
       "MySQL.Aries": "Server=3.140.15.116;Database=AriesContabilidad_Test;Uid=your_username;Pwd=your_password;CharSet=utf8mb4;"
     }
   }
   ```

2. To switch between databases, modify the `Database` parameter in the connection string.

### Best Practices

1. **Backup Before Cloning**
   ```sql
   -- Create backup of production database
   mysqldump -h 3.140.15.116 -u your_username -p AriesContabilidad > aries_backup_$(date +%Y%m%d).sql
   ```

2. **Test Database Naming Convention**
   - Use suffix `_Test` for test databases
   - Include date in name for temporary databases: `AriesContabilidad_Test_20240315`

3. **Security Considerations**
   - Never store production credentials in version control
   - Use different credentials for test database
   - Regularly rotate passwords
   - Limit IP access to the database server

### Useful Commands

1. **Check Database Size**
   ```sql
   SELECT 
     table_schema AS 'Database',
     ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'Size (MB)'
   FROM information_schema.tables
   WHERE table_schema IN ('AriesContabilidad', 'AriesContabilidad_Test')
   GROUP BY table_schema;
   ```

2. **Compare Table Structures**
   ```sql
   SELECT 
     table_name,
     (SELECT GROUP_CONCAT(column_name)
      FROM information_schema.columns
      WHERE table_schema = 'AriesContabilidad'
      AND table_name = t1.table_name) AS prod_columns,
     (SELECT GROUP_CONCAT(column_name)
      FROM information_schema.columns
      WHERE table_schema = 'AriesContabilidad_Test'
      AND table_name = t1.table_name) AS test_columns
   FROM information_schema.tables t1
   WHERE table_schema = 'AriesContabilidad';
   ```

3. **Monitor Connections**
   ```sql
   SHOW PROCESSLIST;
   ```

### Troubleshooting

1. **Connection Issues**
   - Verify IP whitelist settings
   - Check VPN connection if required
   - Confirm credentials are correct
   - Verify port 3306 is not blocked

2. **Cloning Issues**
   - Ensure sufficient disk space
   - Check user privileges
   - Monitor long-running queries
   - Use `SHOW PROCESSLIST` to track progress

3. **Performance Issues**
   - Index usage: `SHOW INDEX FROM table_name`
   - Slow queries: `SHOW FULL PROCESSLIST`
   - Connection limits: `SHOW VARIABLES LIKE 'max_connections'`

### Support

For additional support or access requests:
1. Contact database administrator
2. Check AWS RDS console for monitoring
3. Review application logs for connection issues 