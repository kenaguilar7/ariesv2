-- =============================================================================
-- Aries Contabilidad 2.0 Database Schema and Initialization
-- =============================================================================
-- Author: System Administrator
-- Description: This file contains the database schema and initialization for Aries Contabilidad 2.0
-- Last Updated: 2024
-- =============================================================================

-- -----------------------------------------------------
-- Initial Setup
-- -----------------------------------------------------
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
SET @OLD_TIME_ZONE=@@TIME_ZONE;
SET TIME_ZONE='+00:00';

-- Create database if not exists
CREATE DATABASE IF NOT EXISTS AriesContabilidad_Local /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE AriesContabilidad_Local;

-- Set default character set and collation
ALTER DATABASE AriesContabilidad_Local CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Create EF Migrations History table
CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
    MigrationId varchar(150) NOT NULL,
    ProductVersion varchar(32) NOT NULL,
    PRIMARY KEY (MigrationId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `users` (
    `user_id` int unsigned NOT NULL AUTO_INCREMENT,
    `user_name` varchar(20) NOT NULL DEFAULT 'USER',
    `user_type` enum('Usuario','Administrador') NOT NULL DEFAULT 'Usuario',
    `number_id` varchar(20) NOT NULL,
    `name` varchar(50) NOT NULL,
    `lastname_p` varchar(50) DEFAULT NULL COMMENT 'FATHERS NAME',
    `lastname_m` varchar(50) DEFAULT NULL COMMENT 'MOTHERS NAME',
    `phone_number` varchar(50) DEFAULT NULL,
    `mail` varchar(50) DEFAULT NULL,
    `notes` varchar(100) DEFAULT NULL,
    `password` varchar(50) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int DEFAULT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`user_id`),
    UNIQUE KEY `number_id` (`number_id`),
    UNIQUE KEY `user_name` (`user_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `modules`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `modules` (
    `module_id` int unsigned NOT NULL AUTO_INCREMENT,
    `internal_name` varchar(50) DEFAULT NULL,
    `external_name` varchar(50) DEFAULT NULL,
    `users` enum('Usuario','Administrador') NOT NULL DEFAULT 'Usuario',
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    `deleted` tinyint(1) NOT NULL DEFAULT '0',
    PRIMARY KEY (`module_id`),
    UNIQUE KEY `internal_name` (`internal_name`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `modules_ibfk_1` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `companies`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `companies` (
    `company_id` varchar(5) NOT NULL,
    `type_id` int unsigned NOT NULL,
    `number_id` varchar(20) NOT NULL,
    `name` varchar(50) NOT NULL,
    `money_type` enum('Colones y Dolares','Colones','Dolares') NOT NULL DEFAULT 'Colones y Dolares',
    `op1` varchar(50) DEFAULT NULL,
    `op2` varchar(50) DEFAULT NULL,
    `address` varchar(100) DEFAULT NULL,
    `website` varchar(50) DEFAULT NULL,
    `mail` varchar(50) DEFAULT NULL,
    `phone_number1` varchar(50) DEFAULT NULL,
    `phone_number2` varchar(50) DEFAULT NULL,
    `notes` varchar(100) DEFAULT NULL,
    `user_id` int unsigned DEFAULT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`company_id`),
    UNIQUE KEY `number_id` (`number_id`),
    KEY `type_id` (`type_id`),
    KEY `user_id` (`user_id`),
    CONSTRAINT `companies_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `windows`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `windows` (
    `window_id` int unsigned NOT NULL AUTO_INCREMENT,
    `module_id` int unsigned NOT NULL,
    `internal_name` varchar(50) NOT NULL,
    `external_name` varchar(50) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`window_id`),
    UNIQUE KEY `internal_name` (`internal_name`),
    KEY `module_id` (`module_id`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `windows_ibfk_1` FOREIGN KEY (`module_id`) REFERENCES `modules` (`module_id`),
    CONSTRAINT `windows_ibfk_2` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `windows_permission`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `windows_permission` (
    `window_permission_id` int unsigned NOT NULL AUTO_INCREMENT,
    `user_id` int unsigned NOT NULL,
    `window_id` int unsigned NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`window_permission_id`),
    KEY `user_id` (`user_id`),
    KEY `window_id` (`window_id`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `windows_permission_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
    CONSTRAINT `windows_permission_ibfk_2` FOREIGN KEY (`window_id`) REFERENCES `windows` (`window_id`),
    CONSTRAINT `windows_permission_ibfk_3` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `companies_permission`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `companies_permission` (
    `company_permission_id` int unsigned NOT NULL AUTO_INCREMENT,
    `user_id` int unsigned NOT NULL,
    `company_id` varchar(5) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`company_permission_id`),
    KEY `user_id` (`user_id`),
    KEY `company_id` (`company_id`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `companies_permission_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
    CONSTRAINT `companies_permission_ibfk_2` FOREIGN KEY (`company_id`) REFERENCES `companies` (`company_id`),
    CONSTRAINT `companies_permission_ibfk_3` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `accounts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `accounts` (
    `account_id` int unsigned NOT NULL AUTO_INCREMENT,
    `company_id` varchar(5) NOT NULL,
    `account_code` varchar(20) NOT NULL,
    `account_name` varchar(100) NOT NULL,
    `account_type` enum('Activo','Pasivo','Capital','Ingresos','Costos','Gastos') NOT NULL,
    `account_level` int NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`account_id`),
    UNIQUE KEY `company_id_account_code` (`company_id`,`account_code`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `accounts_ibfk_1` FOREIGN KEY (`company_id`) REFERENCES `companies` (`company_id`),
    CONSTRAINT `accounts_ibfk_2` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `accounts_names`
-- -----------------------------------------------------    
DROP TABLE IF EXISTS `accounts_names`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounts_names` (
  `account_name_id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`account_name_id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=9386 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


-- -----------------------------------------------------
-- Table `accounting_entries`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `accounting_entries` (
    `entry_id` int unsigned NOT NULL AUTO_INCREMENT,
    `company_id` varchar(5) NOT NULL,
    `entry_date` date NOT NULL,
    `description` varchar(200) NOT NULL,
    `reference` varchar(50) DEFAULT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`entry_id`),
    KEY `company_id` (`company_id`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `accounting_entries_ibfk_1` FOREIGN KEY (`company_id`) REFERENCES `companies` (`company_id`),
    CONSTRAINT `accounting_entries_ibfk_2` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `transactions_accounting`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `transactions_accounting` (
    `transaction_id` int unsigned NOT NULL AUTO_INCREMENT,
    `entry_id` int unsigned NOT NULL,
    `account_id` int unsigned NOT NULL,
    `debit_amount` decimal(18,2) DEFAULT '0.00',
    `credit_amount` decimal(18,2) DEFAULT '0.00',
    `currency` enum('CRC','USD') NOT NULL DEFAULT 'CRC',
    `exchange_rate` decimal(18,2) DEFAULT '1.00',
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`transaction_id`),
    KEY `entry_id` (`entry_id`),
    KEY `account_id` (`account_id`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `transactions_accounting_ibfk_1` FOREIGN KEY (`entry_id`) REFERENCES `accounting_entries` (`entry_id`),
    CONSTRAINT `transactions_accounting_ibfk_2` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
    CONSTRAINT `transactions_accounting_ibfk_3` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- Table `accounting_months`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `accounting_months` (
    `month_id` int unsigned NOT NULL AUTO_INCREMENT,
    `company_id` varchar(5) NOT NULL,
    `month` int NOT NULL,
    `year` int NOT NULL,
    `status` enum('Open','Closed') NOT NULL DEFAULT 'Open',
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `updated_by` int unsigned NOT NULL,
    `active` tinyint(1) NOT NULL DEFAULT '1',
    PRIMARY KEY (`month_id`),
    UNIQUE KEY `company_id_month_year` (`company_id`,`month`,`year`),
    KEY `updated_by` (`updated_by`),
    CONSTRAINT `accounting_months_ibfk_1` FOREIGN KEY (`company_id`) REFERENCES `companies` (`company_id`),
    CONSTRAINT `accounting_months_ibfk_2` FOREIGN KEY (`updated_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -----------------------------------------------------
-- View `accounting_entries_info`
-- -----------------------------------------------------
CREATE OR REPLACE VIEW `accounting_entries_info` AS
SELECT 
    T2.company_id,
    CONCAT(T2.year, '-', LPAD(T2.month, 2, '0')) as month_report,
    T1.entry_id,
    T4.name AS account_name,
    T0.transaction_id AS JournalEntryLineId,
    ae.reference,
    ae.description as detail,
    ae.entry_date as bill_date,
    T0.debit_amount as debit,
    T0.credit_amount as credit,
    T0.currency as money_type,
    T0.exchange_rate as money_chance,
    CASE 
        WHEN T0.currency = 'USD' THEN 
            CASE 
                WHEN T0.debit_amount > 0 THEN T0.debit_amount
                ELSE T0.credit_amount
            END
        ELSE 0
    END as balance_usd
FROM 
    transactions_accounting T0
    LEFT JOIN accounting_entries ae ON T0.entry_id = ae.entry_id
    LEFT JOIN accounting_entries T1 ON T0.entry_id = T1.entry_id
    LEFT JOIN accounting_months T2 ON ae.company_id = T2.company_id
    LEFT JOIN accounts T3 ON T0.account_id = T3.account_id
    LEFT JOIN accounts_names T4 ON T3.account_name = T4.name
WHERE 
    T0.active = 1 
    AND T1.active = 1 
    AND T2.active = 1
ORDER BY 
    T3.company_id, month_report;

-- -----------------------------------------------------
-- Restore settings
-- -----------------------------------------------------
SET TIME_ZONE=@OLD_TIME_ZONE;
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Comments and Documentation
-- -----------------------------------------------------
/*
Table: companies
Description: Stores information about companies in the system
Columns:
- company_id: Unique identifier for the company (5 characters)
- type_id: Reference to company type
- number_id: Company's legal identification number
- name: Company name
- money_type: Currency type used by the company
- op1, op2: Optional fields for additional information
- address: Company's physical address
- website: Company's website URL
- mail: Company's contact email
- phone_number1, phone_number2: Contact phone numbers
- notes: Additional notes about the company
- user_id: Reference to the user who manages the company
- created_at: Timestamp of record creation
- updated_at: Timestamp of last update
- active: Flag indicating if the company is active (1) or inactive (0)

Table: users
Description: Stores user information and credentials
Columns:
- user_id: Auto-incrementing unique identifier
- user_name: Login username
- user_type: User role (Usuario/Administrador)
- number_id: User's identification number
- name: User's first name
- lastname_p: User's paternal last name
- lastname_m: User's maternal last name
- phone_number: Contact phone number
- mail: Contact email
- notes: Additional notes about the user
- password: User's password (should be hashed in application)
- created_at: Timestamp of record creation
- updated_at: Timestamp of last update
- updated_by: Reference to user who last updated the record (must be UNSIGNED to match user_id)
- active: Flag indicating if the user is active (1) or inactive (0)
*/ 