-- Additional Database Configuration
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;


SET FOREIGN_KEY_CHECKS = 1; 

-- Set timezone for Central America (Costa Rica)
SET GLOBAL time_zone = '-06:00';

-- Set maximum allowed packet size for large transactions
SET GLOBAL max_allowed_packet = 67108864;

-- Enable file per table for better space management
SET GLOBAL innodb_file_per_table = 1;

SET FOREIGN_KEY_CHECKS = 1; 

-- Create user if not exists
CREATE USER IF NOT EXISTS 'kenneth'@'%' IDENTIFIED BY 'aries_pwd';

-- Grant all privileges to the user for the database
GRANT ALL PRIVILEGES ON AriesContabilidad_Local.* TO 'aries_user'@'%';

-- Grant additional permissions that might be needed
GRANT SUPER ON *.* TO 'aries_user'@'%';
GRANT PROCESS ON *.* TO 'aries_user'@'%';
GRANT RELOAD ON *.* TO 'aries_user'@'%';

-- Make sure privileges are applied
FLUSH PRIVILEGES; 