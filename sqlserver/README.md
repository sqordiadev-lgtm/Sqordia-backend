# SQL Server Seed Scripts

This directory contains seed scripts for SQL Server database used in local development.

## Overview

These scripts populate the database with essential data needed to run the application locally:
- Roles and Permissions
- Admin User
- Subscription Plans
- Currencies
- Default Organization
- Questionnaires
- Templates

## Prerequisites

- SQL Server instance running (via Docker Compose or local installation)
- Database created and migrations applied
- SQL Server Management Studio (SSMS) or sqlcmd access

## Usage

### Option 1: Run All Scripts (Recommended)

Execute the master script to run all seed scripts in order:

```sql
-- In SSMS or sqlcmd
:r sqlserver/00_run_all_seeds.sql
```

Or using sqlcmd:

```bash
sqlcmd -S localhost -d SqordiaDb -U sa -P YourPassword -i sqlserver/00_run_all_seeds.sql
```

### Option 2: Run Individual Scripts

Run scripts individually in order:

1. `01_seed_roles_and_permissions.sql` - Creates roles and permissions
2. `02_seed_admin_user.sql` - Creates admin user
3. `03_seed_subscription_plans.sql` - Creates subscription plans
4. `04_seed_currencies.sql` - Creates currency data
5. `05_seed_default_organization.sql` - Creates default organization
6. `06_seed_questionnaires.sql` - Creates questionnaire templates
7. `08_seed_templates.sql` - Creates business plan templates

### Option 3: Using Docker Compose

If using Docker Compose, you can execute the scripts via the SQL Server container:

```bash
docker exec -i sqordia-sqlserver sqlcmd -S localhost -U sa -P "YourPassword" -d SqordiaDb -i /path/to/sqlserver/00_run_all_seeds.sql
```

## Default Credentials

After running the seed scripts, you can log in with:

- **Email**: `admin@sqordia.com`
- **Password**: `Sqordia2025!`

## Idempotency

All scripts are idempotent - they can be run multiple times safely. They check for existing data before inserting to avoid duplicates.

## Script Details

### 01_seed_roles_and_permissions.sql
- Creates 3 roles: Admin, Collaborateur, Lecteur
- Creates 14 permissions
- Assigns permissions to roles

### 02_seed_admin_user.sql
- Creates admin user account
- Assigns Admin role to admin user

### 03_seed_subscription_plans.sql
- Creates 3 subscription plans: Free, Pro, Enterprise
- Sets pricing and feature limits

### 04_seed_currencies.sql
- Creates 20 common currencies (USD, EUR, GBP, etc.)

### 05_seed_default_organization.sql
- Creates default organization
- Assigns admin user to organization

### 06_seed_questionnaires.sql
- Creates "Business Plan - 20 Common Questions" questionnaire
- Includes 20 bilingual questions (English/French)

### 08_seed_templates.sql
- Creates 8 business plan templates across different industries
- Each template includes 6 sections

## Troubleshooting

### Script Execution Errors

If you encounter errors:

1. **Check database connection**: Ensure SQL Server is running and accessible
2. **Verify migrations**: Ensure all migrations have been applied
3. **Check permissions**: Ensure the database user has INSERT permissions
4. **Review error messages**: Check SQL Server error logs for specific issues

### Common Issues

- **Duplicate key errors**: Scripts are idempotent, but if you see duplicate errors, data may already exist
- **Foreign key violations**: Ensure scripts are run in order (use master script)
- **Missing tables**: Ensure migrations have been applied first

## Notes

- All timestamps use `GETUTCDATE()` for UTC time
- All GUIDs use `NEWID()` for SQL Server compatibility
- Scripts use `IF NOT EXISTS` checks for idempotency
- Default admin user ID is `00000000-0000-0000-0000-000000000000`

