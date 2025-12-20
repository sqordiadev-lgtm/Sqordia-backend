-- SQL Server Seed Scripts - Master Script
-- Run this script to execute all seed scripts in order
-- Safe to run multiple times (idempotent)
--
-- Usage:
--   Option 1: Run individual scripts in SSMS
--   Option 2: Use sqlcmd with -i flag for each script
--   Option 3: Use the combined script (see combined_seed.sql)

PRINT '========================================';
PRINT 'SQL Server Database Seeding';
PRINT '========================================';
PRINT '';
PRINT 'This is the master script file.';
PRINT 'For SSMS: Run each numbered script individually in order.';
PRINT 'For sqlcmd: Use the combined_seed.sql file instead.';
PRINT '';
PRINT 'Scripts to run in order:';
PRINT '  1. 01_seed_roles_and_permissions.sql';
PRINT '  2. 02_seed_admin_user.sql';
PRINT '  3. 03_seed_subscription_plans.sql';
PRINT '  4. 04_seed_currencies.sql';
PRINT '  5. 05_seed_default_organization.sql';
PRINT '  6. 06_seed_questionnaires.sql';
PRINT '  7. 08_seed_templates.sql';
PRINT '';
PRINT 'Or use: combined_seed.sql (all scripts combined)';
PRINT '';
PRINT 'After seeding, login with:';
PRINT '  Email: admin@sqordia.com';
PRINT '  Password: Sqordia2025!';
PRINT '';

