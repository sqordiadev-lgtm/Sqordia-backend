-- SQL Server Seed Scripts - Master Script
-- Run this script to execute all seed scripts in order
-- Safe to run multiple times (idempotent)

PRINT '========================================';
PRINT 'Starting SQL Server Database Seeding';
PRINT '========================================';
PRINT '';

-- 1. Seed Roles and Permissions
PRINT '1. Seeding Roles and Permissions...';
:r 01_seed_roles_and_permissions.sql
GO

-- 2. Seed Admin User
PRINT '2. Seeding Admin User...';
:r 02_seed_admin_user.sql
GO

-- 3. Seed Subscription Plans
PRINT '3. Seeding Subscription Plans...';
:r 03_seed_subscription_plans.sql
GO

-- 4. Seed Currencies
PRINT '4. Seeding Currencies...';
:r 04_seed_currencies.sql
GO

-- 5. Seed Default Organization
PRINT '5. Seeding Default Organization...';
:r 05_seed_default_organization.sql
GO

-- 6. Seed Questionnaires
PRINT '6. Seeding Questionnaires...';
:r 06_seed_questionnaires.sql
GO

-- 7. Seed Templates
PRINT '7. Seeding Templates...';
:r 08_seed_templates.sql
GO

PRINT '';
PRINT '========================================';
PRINT 'Database Seeding Completed Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Admin User: admin@sqordia.com';
PRINT 'Password: Sqordia2025!';
PRINT '';

