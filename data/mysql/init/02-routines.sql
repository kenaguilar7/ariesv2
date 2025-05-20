-- =============================================================================
-- Aries Contabilidad 2.0 Database Routines
-- =============================================================================
-- Author: System Administrator
-- Description: This file contains all stored procedures and functions for Aries Contabilidad 2.0
-- Last Updated: 2024
-- =============================================================================

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO';
SET @OLD_TIME_ZONE=@@TIME_ZONE;
SET TIME_ZONE='+00:00';

USE AriesContabilidad_Local;

-- -----------------------------------------------------
-- Views
-- -----------------------------------------------------

-- View: accounting_entries_info
DROP VIEW IF EXISTS `accounting_entries_info`;
CREATE VIEW `accounting_entries_info` AS 
SELECT 
    T2.company_id,
    T2.month_report,
    T1.entry_id,
    T4.name AS account_name,
    T0.transaction_accounting_id AS JournalEntryLineId,
    T0.reference,
    T0.detail,
    T0.bill_date,
    IF(T0.balance_type = 1, T0.balance, 0) AS debit,
    IF(T0.balance_type = 2, T0.balance, 0) AS credit,
    T0.money_type,
    T0.money_chance,
    T0.foreign_amount AS balance_usd
FROM transactions_accounting T0
LEFT JOIN accounting_entries T1 ON T0.accounting_entry_id = T1.accounting_entry_id
LEFT JOIN accounting_months T2 ON T1.accounting_months_id = T2.accounting_months_id
LEFT JOIN accounts T3 ON T0.account_id = T3.account_id
LEFT JOIN accounts_names T4 ON T3.account_name_id = T4.account_name_id
WHERE T0.active = 1 AND T1.active = 1 AND T2.active = 1
ORDER BY T3.company_id, T2.month_report;

-- View: account_info
DROP VIEW IF EXISTS `account_info`;
CREATE VIEW `account_info` AS 
SELECT 
    T0.account_id,
    T3.account_type,
    T3.company_id,
    SUM(IF(T0.balance_type = 1, T0.balance, 0)) AS debito,
    SUM(IF(T0.balance_type = 2, T0.balance, 0)) AS credito,
    SUM(IF(T0.money_type = 2 AND T0.balance_type = 1, T0.balance / T0.money_chance, 0)) AS debito_USD,
    SUM(IF(T0.money_type = 2 AND T0.balance_type = 2, T0.balance / T0.money_chance, 0)) AS credito_USD,
    DATE_FORMAT(T2.month_report, '%Y%m') AS month_report,
    IF(SUM(IF(T1.status = 1, 1, 0)) > 0, 0, 1) AS cuadrado
FROM transactions_accounting T0
LEFT JOIN accounting_entries T1 ON T0.accounting_entry_id = T1.accounting_entry_id
LEFT JOIN accounting_months T2 ON T1.accounting_months_id = T2.accounting_months_id
LEFT JOIN accounts T3 ON T0.account_id = T3.account_id
WHERE T0.active = 1 AND T1.active = 1 AND T2.active = 1
GROUP BY T0.account_id, YEAR(T2.month_report), MONTH(T2.month_report);

-- -----------------------------------------------------
-- Functions
-- -----------------------------------------------------

-- Function: F_GetAccountPathForReport
DELIMITER //
CREATE FUNCTION `F_GetAccountPathForReport`(accountid INTEGER) 
RETURNS TEXT CHARSET latin1
DETERMINISTIC
BEGIN
    DECLARE rv TEXT;
    DECLARE cm CHAR(1);
    DECLARE ch INT;
    
    SET rv = '';
    SET cm = '';
    SET ch = accountid;
    WHILE ch > 0 DO
        SELECT IFNULL(father_account, 0), CONCAT(rv, cm, name)
        INTO ch, rv
        FROM accounts T0 
        INNER JOIN accounts_names T1 USING(account_name_id)
        WHERE account_id = ch;
        SET cm = '>';
    END WHILE;
    RETURN rv;
END //
DELIMITER ;

-- Function: GetAccountName
DELIMITER //
CREATE FUNCTION `GetAccountName`(AccountName VARCHAR(50)) 
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE AccountNameId INT;

    -- Check if account name exists
    SELECT account_name_id INTO AccountNameId
    FROM accounts_names
    WHERE name = AccountName
    LIMIT 1;

    -- If account name doesn't exist, insert new one
    IF AccountNameId IS NULL THEN
        INSERT INTO accounts_names (name)
        VALUES (AccountName);
        
        SET AccountNameId = LAST_INSERT_ID();
    END IF;

    RETURN AccountNameId;
END //
DELIMITER ;

-- Function: GETFULLPATH
DELIMITER //
CREATE FUNCTION `GETFULLPATH`(accountid INTEGER) 
RETURNS TEXT CHARSET latin1
DETERMINISTIC
BEGIN
    DECLARE rv TEXT;
    DECLARE cm CHAR(1);
    DECLARE ch INT;
    
    SET rv = '';
    SET cm = '';
    SET ch = accountid;
    WHILE ch > 0 DO
        SELECT IFNULL(father_account, 0), CONCAT(rv, cm, name)
        INTO ch, rv
        FROM accounts T0 
        INNER JOIN accounts_names T1 USING(account_name_id)
        WHERE account_id = ch;
        SET cm = '>';
    END WHILE;
    RETURN rv;
END //
DELIMITER ;

-- -----------------------------------------------------
-- Stored Procedures
-- -----------------------------------------------------

-- Procedure: SP_GetAllUsers
DELIMITER //
CREATE PROCEDURE `SP_GetAllUsers`()
BEGIN
    SELECT 
        T0.user_id AS 'Id',
        T0.created_at AS 'CreatedAt',
        T0.updated_at AS 'UpdateAt',
        T0.updated_by AS 'UpdatedBy',
        T0.active AS 'Active',
        T0.user_name AS 'UserName',
        T0.user_type AS 'UserType',
        T0.number_id AS 'IdNumber',
        T0.name AS 'Name',
        T0.lastname_p AS 'LastName',
        T0.lastname_m AS 'MiddleName',
        T0.phone_number AS 'PhoneNumber',
        T0.mail AS 'Mail',
        T0.notes AS 'Memo',
        T0.password AS 'Password'
    FROM users T0
    WHERE T0.active = true;
END //
DELIMITER ;

-- Procedure: SP_AccountHasMovements
DELIMITER //
CREATE PROCEDURE `SP_AccountHasMovements`(
    IN AccountId INT,
    IN CompanyId VARCHAR(4)
)
BEGIN
    SELECT 
        IF(COUNT(*) > 0, 1, 0)
    FROM accounting_entries T2 
    INNER JOIN transactions_accounting T0 ON T2.accounting_entry_id = T0.accounting_entry_id 
    INNER JOIN accounts T1 ON T1.account_id = T0.account_id
    WHERE T0.active = 1 
    AND T2.active = 1 
    AND T1.account_id = AccountId 
    AND T1.company_id = CompanyId;
END //
DELIMITER ;

-- Procedure: SP_AuxiliaryAccountsWithBalanceByDateRange
DELIMITER //
CREATE PROCEDURE `SP_AuxiliaryAccountsWithBalanceByDateRange`(
    IN CompanyId VARCHAR(5),
    IN FirstDate VARCHAR(20),
    IN EndDate VARCHAR(20)
)
BEGIN
    SELECT 
        T0.account_id AS 'Id',
        T2.name AS 'Name',
        F_GetAccountPathForReport(T0.account_id) AS 'PathDirection',
        CASE
            WHEN T0.`account_type` + 0 = 1 THEN 'Activo'
            WHEN T0.`account_type` + 0 = 2 THEN 'Pasivo'
            WHEN T0.`account_type` + 0 = 3 THEN 'Patrimonio'
            WHEN T0.`account_type` + 0 = 4 THEN 'Ingreso'
            WHEN T0.`account_type` + 0 = 5 THEN 'CostoVenta'
            WHEN T0.`account_type` + 0 = 6 THEN 'Egreso'
        END AS 'AccountTag',
        CASE
            WHEN T0.`account_guide` + 0 = 1 THEN 'Cuenta_Titulo'
            WHEN T0.`account_guide` + 0 = 2 THEN 'Cuenta_De_Mayor'
            WHEN T0.`account_guide` + 0 = 3 THEN 'Cuenta_Auxiliar'
        END AS 'AccountType',
        CASE
            WHEN T0.`account_type` + 0 = 1 THEN 1 -- activo
            WHEN T0.`account_type` + 0 = 2 THEN 2 -- Pasivo
            WHEN T0.`account_type` + 0 = 3 THEN 2 -- Patrimonio
            WHEN T0.`account_type` + 0 = 4 THEN 2 -- INGRESO
            WHEN T0.`account_type` + 0 = 5 THEN 1 -- COSTO VENTA
            WHEN T0.`account_type` + 0 = 6 THEN 1 -- EGRESO
        END AS 'DebOrCred',
        T0.father_account AS 'FatherAccount',
        COALESCE(T0.previous_balance_c, 0) AS 'PriorBalance',
        COALESCE(T0.previous_balance_d, 0) AS 'PriorBalanceForeign',
        COALESCE(T1.debito, 0) AS 'DebitBalance',
        COALESCE(T1.debito_USD, 0) AS 'DebitBalanceForeign',
        COALESCE(T1.credito, 0) AS 'CreditBalance',
        COALESCE(T1.credito_USD, 0) AS 'CreditBalanceForeign'
    FROM accounts T0
    INNER JOIN accounts_names T2 USING (account_name_id)
    LEFT OUTER JOIN (
        SELECT 
            T1.account_id,
            SUM(T1.debito) AS 'debito',
            SUM(T1.credito) AS 'credito',
            SUM(T1.debito_USD) AS 'debito_USD',
            SUM(T1.credito_USD) AS 'credito_USD'
        FROM account_info T1
        WHERE T1.company_id = CompanyId
        AND (T1.month_report BETWEEN DATE_FORMAT(STR_TO_DATE(FirstDate, '%Y%m'), '%Y%m') 
            AND DATE_FORMAT(STR_TO_DATE(EndDate, '%Y%m'), '%Y%m'))
        GROUP BY T1.account_id
    ) T1 USING (account_id)
    WHERE T0.company_id = CompanyId;
END //
DELIMITER ;

-- Procedure: SP_ClosePeriod
DELIMITER //
CREATE PROCEDURE `SP_ClosePeriod`(
    IN Id INT,
    IN ClosedMySQL INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE accounting_months
    SET
        `closed` = ClosedMySQL,
        `updated_by` = UpdatedBy 
    WHERE `accounting_months_id` = Id;
END //
DELIMITER ;

-- Procedure: SP_DesactivateAccount
DELIMITER //
CREATE PROCEDURE `SP_DesactivateAccount`(
    IN Id INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE `accounts` T0 
    SET `active` = false, 
        T0.`updated_by` = UpdatedBy
    WHERE T0.`account_id` = Id;
END //
DELIMITER ;

-- Procedure: SP_DesactivateJournalEntry
DELIMITER //
CREATE PROCEDURE `SP_DesactivateJournalEntry`(
    IN Id INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE accounting_entries T0 
    SET T0.updated_by = UpdatedBy, 
        T0.active = false
    WHERE T0.accounting_entry_id = Id;
END //
DELIMITER ;

-- Procedure: SP_DesactivateJournalEntryLine
DELIMITER //
CREATE PROCEDURE `SP_DesactivateJournalEntryLine`(
    IN Id INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE transactions_accounting T0 
    SET T0.updated_by = UpdatedBy, 
        T0.active = false
    WHERE T0.transaction_accounting_id = Id;
END //
DELIMITER ;

-- Procedure: SP_EstadoResultadoIntegralReport
DELIMITER //
CREATE PROCEDURE `SP_EstadoResultadoIntegralReport`(
    IN CompanyId VARCHAR(5),
    IN FirstDate VARCHAR(20),
    IN EndDate VARCHAR(20)
)
BEGIN
    SELECT 
        T0.account_id AS 'Id',
        T2.name AS 'Name',
        F_GetAccountPathForReport(T0.account_id) AS 'PathDirection',
        CASE
            WHEN T0.`account_type` + 0 = 1 THEN 'Activo'
            WHEN T0.`account_type` + 0 = 2 THEN 'Pasivo'
            WHEN T0.`account_type` + 0 = 3 THEN 'Patrimonio'
            WHEN T0.`account_type` + 0 = 4 THEN 'Ingreso'
            WHEN T0.`account_type` + 0 = 5 THEN 'CostoVenta'
            WHEN T0.`account_type` + 0 = 6 THEN 'Egreso'
        END AS 'AccountTag',
        CASE
            WHEN T0.`account_guide` + 0 = 1 THEN 'Cuenta_Titulo'
            WHEN T0.`account_guide` + 0 = 2 THEN 'Cuenta_De_Mayor'
            WHEN T0.`account_guide` + 0 = 3 THEN 'Cuenta_Auxiliar'
        END AS 'AccountType',
        CASE
            WHEN T0.`account_type` + 0 = 1 THEN 'Debito' -- activo
            WHEN T0.`account_type` + 0 = 2 THEN 'Credito' -- Pasivo
            WHEN T0.`account_type` + 0 = 3 THEN 'Credito' -- Patrimonio
            WHEN T0.`account_type` + 0 = 4 THEN 'Credito' -- INGRESO
            WHEN T0.`account_type` + 0 = 5 THEN 'Debito' -- COSTO VENTA
            WHEN T0.`account_type` + 0 = 6 THEN 'Debito' -- EGRESO
        END AS 'DebOCred',
        T0.editable AS 'EditableMySql',
        T0.father_account AS 'FatherAccount',
        COALESCE(T0.previous_balance_c, 0) AS 'PriorBalance',
        COALESCE(T0.previous_balance_d, 0) AS 'PriorBalanceForeign',
        COALESCE(T1.debito, 0) AS 'DebitBalance',
        COALESCE(T1.debito_USD, 0) AS 'DebitBalanceForeign',
        COALESCE(T1.credito, 0) AS 'CreditBalance',
        COALESCE(T1.credito_USD, 0) AS 'CreditBalanceForeign'
    FROM accounts T0
    INNER JOIN accounts_names T2 USING (account_name_id)
    LEFT OUTER JOIN (
        SELECT 
            T1.account_id,
            SUM(T1.debito) AS 'debito',
            SUM(T1.credito) AS 'credito',
            SUM(T1.debito_USD) AS 'debito_USD',
            SUM(T1.credito_USD) AS 'credito_USD'
        FROM account_info T1   
        WHERE T1.company_id = CompanyId
        AND (T1.month_report BETWEEN DATE_FORMAT(STR_TO_DATE(FirstDate, '%Y%m'), '%Y%m') 
            AND DATE_FORMAT(STR_TO_DATE(EndDate, '%Y%m'), '%Y%m'))
        AND T1.account_type IN ('INGRESO','EGRESO','COSTO VENTA')
        GROUP BY T1.account_id
    ) T1 USING (account_id)
    WHERE T0.company_id = CompanyId 
    AND T0.account_type IN ('INGRESO','EGRESO','COSTO VENTA');
END //
DELIMITER ;

-- Procedure: SP_FindUserById
DELIMITER //
CREATE PROCEDURE `SP_FindUserById`(
    IN Id INT
)
BEGIN
    SELECT 
        T0.user_id AS 'Id',
        T0.created_at AS 'CreatedAt',
        T0.updated_at AS 'UpdateAt',
        T0.updated_by AS 'UpdatedBy',
        T0.active AS 'Active',
        T0.user_name AS 'UserName',
        T0.user_type AS 'UserType',
        T0.number_id AS 'IdNumber',
        T0.name AS 'Name',
        T0.lastname_p AS 'LastName',
        T0.lastname_m AS 'MiddleName',
        T0.phone_number AS 'PhoneNumber',
        T0.mail AS 'Mail',
        T0.notes AS 'Memo',
        T0.password AS 'Password'
    FROM users T0
    WHERE T0.user_id = Id;
END //
DELIMITER ;

-- Procedure: SP_GetAccountById
DELIMITER //
CREATE PROCEDURE `SP_GetAccountById`(
    IN AccountId INT
)
BEGIN
    SELECT 
        T0.`account_id` AS 'Id',
        T1.`name` AS 'Name',
        T0.`father_account` AS 'FatherAccount',
        T0.`previous_balance_c` AS 'PriorBalance',
        T0.`previous_balance_d` AS 'PriorBalanceForeign',
        T0.`company_id` AS 'CompanyId',
        CASE
            WHEN T0.`account_type`+ 0 = 1 THEN 'Activo'
            WHEN T0.`account_type`+ 0 = 2 THEN 'Pasivo'
            WHEN T0.`account_type`+ 0 = 3 THEN 'Patrimonio'
            WHEN T0.`account_type`+ 0 = 4 THEN 'CostoVenta'
            WHEN T0.`account_type`+ 0 = 5 THEN 'Ingreso'
            WHEN T0.`account_type`+ 0 = 6 THEN 'Egreso'
        END AS 'AccountTag',
        CASE 
            WHEN T0.`account_guide` + 0 = 1 THEN 'Cuenta_Titulo'
            WHEN T0.`account_guide` + 0 = 2 THEN 'Cuenta_De_Mayor'
            WHEN T0.`account_guide` + 0 = 3 THEN 'Cuenta_Auxiliar'
        END AS 'AccountType',
        T0.`editable` AS 'Editable',
        T0.`detail` AS 'Memo',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdatedAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active',
        GETFULLPATH(T0.`account_id`) AS 'PathDirection'
    FROM `accounts` T0 
    LEFT JOIN `accounts_names` T1 USING(account_name_id)
    WHERE T0.`active` = true 
    AND T0.`account_id` = AccountId;
END //
DELIMITER ;

-- Procedure: SP_GetAccountsByCompanyId
DELIMITER //
CREATE PROCEDURE `SP_GetAccountsByCompanyId`(
    IN CompanyId VARCHAR(4)
)
BEGIN
    SELECT 
        T0.`account_id` AS 'Id',
        T1.`name` AS 'Name',
        T0.`father_account` AS 'FatherAccount',
        T0.`previous_balance_c` AS 'PriorBalance',
        T0.`previous_balance_d` AS 'PriorBalanceForeign',
        T0.`company_id` AS 'CompanyId',
        CASE
            WHEN T0.`account_type`+ 0 = 1 THEN 'Activo'
            WHEN T0.`account_type`+ 0 = 2 THEN 'Pasivo'
            WHEN T0.`account_type`+ 0 = 3 THEN 'Patrimonio'
            WHEN T0.`account_type`+ 0 = 4 THEN 'CostoVenta'
            WHEN T0.`account_type`+ 0 = 5 THEN 'Ingreso'
            WHEN T0.`account_type`+ 0 = 6 THEN 'Egreso'
        END AS 'AccountTag',
        CASE 
            WHEN T0.`account_guide` + 0 = 1 THEN 'Cuenta_Titulo'
            WHEN T0.`account_guide` + 0 = 2 THEN 'Cuenta_De_Mayor'
            WHEN T0.`account_guide` + 0 = 3 THEN 'Cuenta_Auxiliar'
        END AS 'AccountType',
        T0.`editable` AS 'Editable',
        T0.`detail` AS 'Memo',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdatedAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active',
        GETFULLPATH(T0.`account_id`) AS 'PathDirection'
    FROM `accounts` T0 
    LEFT JOIN `accounts_names` T1 USING(account_name_id)
    WHERE T0.`active` = true 
    AND T0.`company_id` = CompanyId;
END //
DELIMITER ;

-- Procedure: SP_GetAllJournalEntryLineByAccountId
DELIMITER //
CREATE PROCEDURE `SP_GetAllJournalEntryLineByAccountId`(
    IN AccountId INT
)
BEGIN
    SELECT 
        T0.`transaction_accounting_id` AS 'Id',
        T0.`account_id` AS 'AccountId',
        T0.`accounting_entry_id` AS 'JournalEntryId',
        GETFULLPATH(T0.`account_id`) AS 'AccountPath',
        T0.`reference` AS 'Reference',
        T0.`detail` AS 'Memo',
        T0.`balance` AS 'Amount',
        T0.`foreign_amount` AS 'ForeignAmount',
        T0.`balance_type` AS 'DebOCred',
        T0.`money_type` AS 'Currency',
        T0.`money_chance` AS 'Rate',
        T0.`bill_date` AS 'Date',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active'
    FROM `transactions_accounting` T0 
    INNER JOIN `accounting_entries` T1 USING (accounting_entry_id)  
    WHERE T0.`active` = true 
    AND T1.`active` 
    AND T0.`account_id` = AccountId;
END //
DELIMITER ;

-- Procedure: SP_GetAllJournalEntyLineByAccoudIdAndPostingPeriodId
DELIMITER //
CREATE PROCEDURE `SP_GetAllJournalEntyLineByAccoudIdAndPostingPeriodId`(
    IN accountId INT,
    IN postingPeriodId INT
)
BEGIN
    SELECT 
        T0.`transaction_accounting_id` AS 'Id',
        T0.`account_id` AS 'AccountId',
        T0.`accounting_entry_id` AS 'JournalEntryId',
        GETFULLPATH(T0.`account_id`) AS 'AccountPath',
        T0.`reference` AS 'Reference',
        T0.`detail` AS 'Memo',
        T0.`balance` AS 'Amount',
        T0.`foreign_amount` AS 'ForeignAmount',
        T0.`balance_type` AS 'DebOCred',
        T0.`money_type` AS 'Currency',
        T0.`money_chance` AS 'Rate',
        T0.`bill_date` AS 'Date',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active'
    FROM `transactions_accounting` T0 
    INNER JOIN `accounting_entries` T1 USING (accounting_entry_id) 
    INNER JOIN `accounting_months` T2 USING(accounting_months_id)
    WHERE (T0.`active` = true AND T1.`active` = true)
    AND T0.`account_id` = accountId 
    AND T2.`accounting_months_id` = postingPeriodId;
END //
DELIMITER ;

-- Procedure: SP_GetAllPostingPeriod
DELIMITER //
CREATE PROCEDURE `SP_GetAllPostingPeriod`(
    IN CompanyId VARCHAR(4)
)
BEGIN
    SELECT 
        T0.`accounting_months_id` AS 'Id',
        T0.`month_report` AS 'Date',
        YEAR(`month_report`) AS 'Year',
        MONTH(`month_report`) AS 'Month',
        T0.`closed` AS 'Closed',
        T0.`company_id` AS 'CompanyId',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active'
    FROM `accounting_months` T0 
    WHERE T0.`active` = true 
    AND T0.`company_id` = CompanyId;
END //
DELIMITER ;

-- Procedure: SP_GetClosingPostingPeriodReport
DELIMITER //
CREATE PROCEDURE `SP_GetClosingPostingPeriodReport`(
    IN CompanyId VARCHAR(4)
)
BEGIN
    SELECT 
        T0.Id,
        T0.company_id AS 'CompanyId',
        T0.from_period_id AS 'FromPeriodId',
        T0.to_period_id AS 'ToPeriodId',
        T0.from_period AS 'FromPeriod',
        T0.to_period AS 'ToPeriod',
        T0.amount AS 'Amount',
        T0.user_notes AS 'UserNotes',
        T1.user_name AS 'CreatedBy',
        T0.created_at AS 'CreatedAt',
        T0.updated_at AS 'UpdateAt'
    FROM posting_period_end_closing T0 
    LEFT OUTER JOIN users T1 ON T0.updated_by = T1.user_id 
    WHERE T0.company_id = CompanyId;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryById
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryById`(
    IN Id INT
)
BEGIN
    SELECT
        T0.`accounting_entry_id` AS 'Id',
        T0.`entry_id` AS 'Number',
        T0.`accounting_months_id` AS 'PostingPeriodId',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`status` AS 'JournalEntryStatus',
        T0.`active` AS 'Active'
    FROM `accounting_entries` T0
    WHERE T0.active = true 
    AND T0.`accounting_entry_id` = Id;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryByPostingPeriodId
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryByPostingPeriodId`(
    IN PostingPeriodId INT
)
BEGIN
    SELECT
        T0.`accounting_entry_id` AS 'Id',
        T0.`entry_id` AS 'Number',
        T0.`accounting_months_id` AS 'PostingPeriodId',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        CASE
            WHEN T0.`status` + 0 = 1 THEN 'Progress'
            WHEN T0.`status` + 0 = 2 THEN 'Approved'
            WHEN T0.`status` + 0 = 3 THEN 'Convalidated'
        END AS 'JournalEntryStatus',
        T0.`active` AS 'Active'
    FROM `accounting_entries` T0
    WHERE T0.active = true 
    AND T0.`accounting_months_id` = PostingPeriodId;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryConsecutive
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryConsecutive`(
    IN postingPeriodId INT
)
BEGIN
    SELECT IF(MAX(entry_id) + 1 is null, 1, MAX(entry_id)+1) 
    FROM accounting_entries T0 
    WHERE accounting_months_id = postingPeriodId;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryDeletedBydDateRange
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryDeletedBydDateRange`(
    IN CompanyId VARCHAR(5),
    IN FirstDate VARCHAR(20),
    IN EndDate VARCHAR(20)
)
BEGIN
    SELECT 
        DATE_FORMAT(T2.month_report, '%M %y') AS 'PostingPeriodName',
        T1.accounting_entry_id AS 'JournalEntryId',
        T1.entry_id AS 'JournalEntryNumber',
        T1.updated_at AS 'DeletedAt',
        T5.user_name AS 'DeletedBy',
        T0.DebitAmount, 
        T0.CreditAmount
    FROM accounting_entries T1 
    LEFT JOIN (
        SELECT 
            accounting_entry_id, 
            SUM(IF(T0.balance_type = 1, T0.balance, 0)) AS DebitAmount,
            SUM(IF(T0.balance_type = 2, T0.balance, 0)) AS CreditAmount 
        FROM transactions_accounting T0 
        WHERE T0.active = 1 
        GROUP BY accounting_entry_id
    ) T0 USING(accounting_entry_id)
    LEFT JOIN accounting_months T2 USING(accounting_months_id) 
    LEFT JOIN users T5 ON T1.updated_by = T5.user_id
    WHERE T2.company_id = CompanyId 
    AND T1.active = 0
    AND (DATE_FORMAT(T2.month_report, '%Y%m') BETWEEN 
        DATE_FORMAT(STR_TO_DATE(FirstDate, '%Y%m'), '%Y%m') AND 
        DATE_FORMAT(STR_TO_DATE(EndDate, '%Y%m'), '%Y%m'))
    ORDER BY T2.month_report, T1.accounting_entry_id;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryLineById
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryLineById`(
    IN Id INT
)
BEGIN
    SELECT 
        T0.`transaction_accounting_id` AS 'Id',
        T0.`account_id` AS 'AccountId',
        T0.`accounting_entry_id` AS 'JournalEntryId',
        GETFULLPATH(T0.`account_id`) AS 'AccountPath',
        T0.`reference` AS 'Reference',
        T0.`detail` AS 'Memo',
        T0.`balance` AS 'Amount',
        T0.`foreign_amount` AS 'ForeignAmount',
        T0.`balance_type` AS 'DebOCred',
        T0.`money_type` AS 'Currency',
        T0.`money_chance` AS 'Rate',
        T0.`bill_date` AS 'Date',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active'
    FROM `transactions_accounting` T0 
    WHERE T0.`active` = true 
    AND T0.`transaction_accounting_id` = Id;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntryLineByJournalEntryId
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntryLineByJournalEntryId`(
    IN JournalEntryId INT
)
BEGIN
    SELECT 
        T0.`transaction_accounting_id` AS 'Id',
        T0.`account_id` AS 'AccountId',
        T0.`accounting_entry_id` AS 'JournalEntryId',
        GETFULLPATH(T0.`account_id`) AS 'AccountPath',
        T3.name AS 'AccountName',
        T0.`reference` AS 'Reference',
        T0.`detail` AS 'Memo',
        T0.`balance` AS 'Amount',
        T0.`foreign_amount` AS 'ForeignAmount',
        T0.`balance_type` AS 'DebOrCred',
        T0.`money_type` AS 'Currency',
        T0.`money_chance` AS 'RateAmount',
        T0.`bill_date` AS 'Date',
        T0.`created_at` AS 'CreatedAt',
        T0.`updated_at` AS 'UpdateAt',
        T0.`updated_by` AS 'UpdatedBy',
        T0.`active` AS 'Active'
    FROM `transactions_accounting` T0
    LEFT JOIN accounts T2 ON T0.account_id = T2.account_id
    LEFT JOIN accounts_names T3 ON T2.account_name_id = T3.account_name_id
    WHERE T0.`active` = true 
    AND T0.`accounting_entry_id` = JournalEntryId;
END //
DELIMITER ;

-- Procedure: SP_GetJournalEntyLineDeletedByDateRange
DELIMITER //
CREATE PROCEDURE `SP_GetJournalEntyLineDeletedByDateRange`(
    IN CompanyId VARCHAR(5),
    IN FirstDate VARCHAR(20),
    IN EndDate VARCHAR(20)
)
BEGIN
    SET lc_time_names = 'es_MX';
    SELECT 
        T0.transaction_accounting_id AS 'JournalEntryLineId',
        DATE_FORMAT(T2.month_report, '%M %y') AS 'PostingPeriodName',
        T1.entry_id AS 'JournalEntryNumber',
        T4.name AS 'AccountName',
        T0.reference AS 'Reference',
        T0.detail AS 'Memo',
        T0.bill_date AS 'DocDate',
        IF(T0.balance_type = 1, T0.balance, 0) AS DebitAmount,
        IF(T0.balance_type = 2, T0.balance, 0) AS CreditAmount,
        T0.money_type AS 'Currency',
        T0.money_chance AS 'RateAmount',
        T0.foreign_amount AS 'ForeignAmount',
        T0.updated_at AS 'DeletedAt',
        T5.user_name AS 'DeletedBy'
    FROM transactions_accounting T0 
    INNER JOIN accounting_entries T1 USING(accounting_entry_id) 
    INNER JOIN accounting_months T2 USING(accounting_months_id) 
    INNER JOIN accounts T3 USING (account_id)
    INNER JOIN accounts_names T4 USING (account_name_id)
    LEFT JOIN users T5 ON T0.updated_by = T5.user_id
    WHERE T2.company_id = CompanyId 
    AND T0.active = 0
    AND (FirstDate = '' OR DATE_FORMAT(T2.month_report, '%Y%m') BETWEEN 
        DATE_FORMAT(STR_TO_DATE(FirstDate, '%Y%m'), '%Y%m') AND 
        DATE_FORMAT(STR_TO_DATE(EndDate, '%Y%m'), '%Y%m'))
    ORDER BY T2.month_report, T1.entry_id, T0.transaction_accounting_id;
END //
DELIMITER ;

-- Procedure: SP_GetOrCreateAccountName
DELIMITER //
CREATE PROCEDURE `SP_GetOrCreateAccountName`(
    IN AccountName VARCHAR(50),
    OUT Id INT
)
BEGIN
    -- Check if account name exists
    SELECT `account_name_id` INTO Id
    FROM `accounts_names`
    WHERE `name` = AccountName
    LIMIT 1;

    -- If account name doesn't exist, insert new one
    IF Id IS NULL THEN
        INSERT INTO `accounts_names` (`name`)
        VALUES (AccountName);
        
        SET Id = LAST_INSERT_ID();
    END IF;
END //
DELIMITER ;

-- Procedure: SP_GetPostingPeriodReport
DELIMITER //
CREATE PROCEDURE `SP_GetPostingPeriodReport`(
    IN CompanyId VARCHAR(4)
)
BEGIN
    SET lc_time_names = 'es_ES';
    SELECT 
        DATE_FORMAT(ac.month_report,'%M %Y') AS 'PostingPeriodDateString',
        IF(ac.closed, 'Cerrado','Abierto') AS 'Status',
        DATE_FORMAT(ac.created_at,'%d %M %Y') AS 'CreatedDateString',
        IF(ac.closed,DATE_FORMAT(ac.updated_at,'%d %M %Y'), '') AS 'ClosedDateString',
        (SELECT user_name FROM users us WHERE us.user_id = ac.updated_by LIMIT 1) AS 'UserName'
    FROM accounting_months ac 
    WHERE ac.company_id = CompanyId 
    AND ac.active = 1 
    ORDER BY ac.month_report DESC;
END //
DELIMITER ;

-- Procedure: SP_InsertAccount
DELIMITER //
CREATE PROCEDURE `SP_InsertAccount`(
    IN Name INT,
    IN FatherAccount INT,
    IN CompanyId VARCHAR(4),
    IN AccountType INT,
    IN AccountTag INT,
    IN Editable TINYINT(1),
    IN Memo VARCHAR(50),
    IN UpdatedBy INT,
    OUT Id INT
)
BEGIN
    INSERT INTO `accounts` (
        `account_name_id`, `father_account`, `company_id`, `account_type`,
        `account_guide`, `editable`, `detail`,
        `created_at`, `updated_at`, `updated_by`
    )
    VALUES (
        Name, FatherAccount, CompanyId, AccountTag,
        AccountType, Editable, Memo,
        NOW(), NOW(), UpdatedBy
    );
    
    SET Id = LAST_INSERT_ID();
END //
DELIMITER ;

-- Procedure: SP_InsertClosingPostingPeriod
DELIMITER //
CREATE PROCEDURE `SP_InsertClosingPostingPeriod`(
    IN CompanyId VARCHAR(4),
    IN FromPeriodId INT,
    IN ToPeriodId INT,
    IN FromPeriod VARCHAR(50),
    IN ToPeriod VARCHAR(50),
    IN Amount DOUBLE(18,2),
    IN UserNotes VARCHAR(100),
    IN UpdatedBy INT,
    OUT Id INT
)
BEGIN
    INSERT INTO posting_period_end_closing (
        company_id, from_period_id, to_period_id, from_period,
        to_period, amount, user_notes, updated_by
    ) 
    VALUES (
        CompanyId, FromPeriodId, ToPeriodId, FromPeriod,
        ToPeriod, Amount, UserNotes, UpdatedBy
    );
    
    SET Id = LAST_INSERT_ID();
END //
DELIMITER ;

-- Procedure: SP_InsertCompany
DELIMITER //
CREATE PROCEDURE `SP_InsertCompany`(
    IN Code VARCHAR(5),
    IN TypeId INT,
    IN NumberId VARCHAR(20),
    IN CompanyName VARCHAR(50),
    IN MoneyType ENUM('Colones y Dolares', 'Colones', 'Dolares'),
    IN Op1 VARCHAR(50),
    IN Op2 VARCHAR(50),
    IN Address VARCHAR(100),
    IN Website VARCHAR(50),
    IN Mail VARCHAR(50),
    IN PhoneNumber1 VARCHAR(50),
    IN PhoneNumber2 VARCHAR(50),
    IN Notes VARCHAR(100),
    IN UserId INT,
    IN IsActive TINYINT(1),
    OUT NewCompanyId VARCHAR(5)
)
BEGIN
    INSERT INTO `companies` (
        `company_id`, `type_id`, `number_id`, `name`,
        `money_type`, `op1`, `op2`, `address`,
        `website`, `mail`, `phone_number1`, `phone_number2`,
        `notes`, `user_id`, `created_at`, `updated_at`, `active`
    )
    VALUES (
        Code, TypeId, NumberId, CompanyName,
        MoneyType, Op1, Op2, Address,
        Website, Mail, PhoneNumber1, PhoneNumber2,
        Notes, UserId, NOW(), NOW(), IsActive
    );
    
    SET NewCompanyId = Code;
END //
DELIMITER ;

-- Procedure: SP_InsertJournalEntry
DELIMITER //
CREATE PROCEDURE `SP_InsertJournalEntry`(
    IN Number INT,
    IN PostingPeriodId INT,
    IN UpdatedBy INT,
    IN JournalEntryStatus INT,
    OUT Id INT
)
BEGIN
    INSERT INTO `accounting_entries` (
        `entry_id`, `accounting_months_id`, `updated_by`, `status`
    )
    VALUES (
        Number, PostingPeriodId, UpdatedBy, JournalEntryStatus
    );
    
    SET Id = LAST_INSERT_ID();
END //
DELIMITER ;

-- Procedure: SP_InsertJournalEntryLine
DELIMITER //
CREATE PROCEDURE `SP_InsertJournalEntryLine`(
    IN AccountId INT,
    IN JournalEntryId INT,
    IN Reference VARCHAR(100),
    IN Memo VARCHAR(100),
    IN Amount DOUBLE(18,2),
    IN ForeignAmount DOUBLE(18,2),
    IN DebOrCred INT,
    IN Currency INT,
    IN RateAmount DOUBLE(8,2),
    IN Date DATE,
    IN UpdatedBy INT,
    OUT Id INT
)
BEGIN
    INSERT INTO `transactions_accounting` (
        `account_id`, `accounting_entry_id`, `reference`, `detail`,
        `balance`, `foreign_amount`, `balance_type`, `money_type`,
        `money_chance`, `bill_date`, `updated_by`
    )
    VALUES (
        AccountId, JournalEntryId, Reference, Memo,
        Amount, ForeignAmount, DebOrCred, Currency,
        RateAmount, Date, UpdatedBy
    );
    
    SET Id = LAST_INSERT_ID();
END //
DELIMITER ;

-- Procedure: SP_InsertPostingPeriod
DELIMITER //
CREATE PROCEDURE `SP_InsertPostingPeriod`(
    IN Date DATE,
    IN ClosedMySQL INT,
    IN CompanyId VARCHAR(4),
    IN UpdatedBy INT,
    OUT Id INT
)
BEGIN
    INSERT INTO `accounting_months` (
        `month_report`, `closed`, `company_id`, `updated_by`
    )
    VALUES (
        Date, ClosedMySQL, CompanyId, UpdatedBy
    );

    SET Id = LAST_INSERT_ID();
END //
DELIMITER ;

-- Procedure: SP_JournalEntryReportByDateRange
DELIMITER //
CREATE PROCEDURE `SP_JournalEntryReportByDateRange`(
    IN CompanyId VARCHAR(5),
    IN FirstDate VARCHAR(20),
    IN EndDate VARCHAR(20)
)
BEGIN
    SET lc_time_names = 'es_MX';
    SELECT 
        DATE_FORMAT(month_report, '%M %y') AS 'PostingPeriodName',
        entry_id AS 'JournalEntryNumber',
        account_name AS 'AccountName',
        reference AS 'Reference',
        detail AS 'Memo',
        bill_date AS 'DocDate',
        debit AS 'DebitAmount',
        credit AS 'CreditAmount',
        money_type AS 'Currency',
        money_chance AS 'RateAmount',
        balance_usd AS 'ForeignAmount'
    FROM accounting_entries_info
    WHERE company_id = CompanyId 
    AND (FirstDate = '' OR DATE_FORMAT(month_report, '%Y%m') BETWEEN 
        DATE_FORMAT(STR_TO_DATE(FirstDate, '%Y%m'), '%Y%m') AND 
        DATE_FORMAT(STR_TO_DATE(EndDate, '%Y%m'), '%Y%m'))
    ORDER BY month_report, entry_id;
END //
DELIMITER ;

-- Procedure: SP_RestoreJournalEntry
DELIMITER //
CREATE PROCEDURE `SP_RestoreJournalEntry`(
    IN Id INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE accounting_entries T0 
    SET T0.active = true,
        T0.updated_by = UpdatedBy,
        T0.updated_at = NOW()
    WHERE T0.accounting_entry_id = Id;
END //
DELIMITER ;

-- Procedure: SP_RestoreJournalEntryLine
DELIMITER //
CREATE PROCEDURE `SP_RestoreJournalEntryLine`(
    IN Id INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE transactions_accounting T0 
    SET T0.active = 1,
        T0.updated_by = UpdatedBy
    WHERE T0.transaction_accounting_id = Id;
END //
DELIMITER ;

-- Procedure: SP_UpdateAccount
DELIMITER //
CREATE PROCEDURE `SP_UpdateAccount`(
    IN Id INT,
    IN AccountType INT,
    IN UpdatedBy INT
)
BEGIN
    UPDATE `accounts` T0 
    SET T0.`account_guide` = AccountType,
        T0.`updated_by` = UpdatedBy
    WHERE T0.`account_id` = Id;
END //
DELIMITER ;

-- Procedure: SP_UpdateJournalEntry
DELIMITER //
CREATE PROCEDURE `SP_UpdateJournalEntry`(
    IN Id INT,
    IN UpdatedBy INT,
    IN JournalEntryStatus INT,
    IN Number INT,
    IN PostingPeriodId INT
)
BEGIN
    UPDATE accounting_entries T0 
    SET T0.updated_by = UpdatedBy,
        T0.status = JournalEntryStatus,
        T0.entry_id = Number,
        T0.accounting_months_id = PostingPeriodId
    WHERE T0.accounting_entry_id = Id;
END //
DELIMITER ;

-- Procedure: SP_UpdateJournalEntryLine
DELIMITER //
CREATE PROCEDURE `SP_UpdateJournalEntryLine`(
    IN Id INT,
    IN AccountId INT,
    IN Reference VARCHAR(100),
    IN Memo VARCHAR(100),
    IN Amount DOUBLE(18,2),
    IN ForeignAmount DOUBLE(18,2),
    IN DebOrCred INT,
    IN Currency INT,
    IN RateAmount DOUBLE(8,2),
    IN Date DATE,
    IN UpdatedBy INT
)
BEGIN
    UPDATE transactions_accounting T0 
    SET T0.account_id = AccountId,
        T0.reference = Reference,
        T0.detail = Memo,
        T0.balance = Amount,
        T0.foreign_amount = ForeignAmount,
        T0.balance_type = DebOrCred,
        T0.money_type = Currency,
        T0.money_chance = RateAmount,
        T0.bill_date = Date,
        T0.updated_by = UpdatedBy
    WHERE T0.transaction_accounting_id = Id;
END //
DELIMITER ;

-- Procedure: SP_UpdatePartlyAccount
DELIMITER //
CREATE PROCEDURE `SP_UpdatePartlyAccount`(
    IN Id INT,
    IN NameMySql VARCHAR(50),
    IN Memo VARCHAR(50),
    IN UpdatedBy INT,
    IN CompanyId VARCHAR(4)
)
BEGIN
    SET @accountNameId = 0;
    
    INSERT IGNORE INTO accounts_names(name) VALUE(NameMySql);
    
    SET @accountNameId = (
        SELECT account_name_id 
        FROM accounts_names 
        WHERE name = NameMySql 
        LIMIT 1
    );
    
    UPDATE accounts 
    SET account_name_id = @accountNameId,
        detail = Memo,
        updated_by = UpdatedBy
    WHERE account_id = Id
    AND company_id = CompanyId;
END //
DELIMITER ;

-- Reset MySQL session variables
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
SET TIME_ZONE=@OLD_TIME_ZONE;
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT;
SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS;
SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION;
SET SQL_NOTES=@OLD_SQL_NOTES; 