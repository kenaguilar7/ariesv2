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