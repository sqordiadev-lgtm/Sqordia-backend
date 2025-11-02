-- Seed Azure SQL Database - WORKING PARTS ONLY
-- These sections executed successfully:
-- 1. Roles creation (4 rows)
-- 2. Permissions creation (15 rows)
-- 3. Role-Permission assignments (all 4 roles)
-- 4. Organization creation (1 row)

-- 1. Create Roles
INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole])
VALUES 
    (CAST('6FE80855-70FF-4863-92B1-7EE266426DEE' AS UNIQUEIDENTIFIER), 'Admin', 'System Administrator with full access', 1),
    (CAST('7FE80855-70FF-4863-92B1-7EE266426DEF' AS UNIQUEIDENTIFIER), 'User', 'Regular user with basic access', 1),
    (CAST('8FE80855-70FF-4863-92B1-7EE266426DEA' AS UNIQUEIDENTIFIER), 'OrganizationAdmin', 'Organization Administrator', 0),
    (CAST('9FE80855-70FF-4863-92B1-7EE266426DEB' AS UNIQUEIDENTIFIER), 'OrganizationMember', 'Organization Member', 0);

-- 2. Create Permissions
INSERT INTO [Permissions] ([Id], [Name], [Description], [Category])
VALUES 
    -- User Management
    (CAST('A1E80855-70FF-4863-92B1-7EE266426DE1' AS UNIQUEIDENTIFIER), 'Users.Create', 'Create new users', 'UserManagement'),
    (CAST('A2E80855-70FF-4863-92B1-7EE266426DE2' AS UNIQUEIDENTIFIER), 'Users.Read', 'View users', 'UserManagement'),
    (CAST('A3E80855-70FF-4863-92B1-7EE266426DE3' AS UNIQUEIDENTIFIER), 'Users.Update', 'Update users', 'UserManagement'),
    (CAST('A4E80855-70FF-4863-92B1-7EE266426DE4' AS UNIQUEIDENTIFIER), 'Users.Delete', 'Delete users', 'UserManagement'),
    
    -- Organization Management
    (CAST('B1E80855-70FF-4863-92B1-7EE266426DE1' AS UNIQUEIDENTIFIER), 'Organizations.Create', 'Create organizations', 'OrganizationManagement'),
    (CAST('B2E80855-70FF-4863-92B1-7EE266426DE2' AS UNIQUEIDENTIFIER), 'Organizations.Read', 'View organizations', 'OrganizationManagement'),
    (CAST('B3E80855-70FF-4863-92B1-7EE266426DE3' AS UNIQUEIDENTIFIER), 'Organizations.Update', 'Update organizations', 'OrganizationManagement'),
    (CAST('B4E80855-70FF-4863-92B1-7EE266426DE4' AS UNIQUEIDENTIFIER), 'Organizations.Delete', 'Delete organizations', 'OrganizationManagement'),
    
    -- Business Plan Management
    (CAST('C1E80855-70FF-4863-92B1-7EE266426DE1' AS UNIQUEIDENTIFIER), 'BusinessPlans.Create', 'Create business plans', 'BusinessPlanManagement'),
    (CAST('C2E80855-70FF-4863-92B1-7EE266426DE2' AS UNIQUEIDENTIFIER), 'BusinessPlans.Read', 'View business plans', 'BusinessPlanManagement'),
    (CAST('C3E80855-70FF-4863-92B1-7EE266426DE3' AS UNIQUEIDENTIFIER), 'BusinessPlans.Update', 'Update business plans', 'BusinessPlanManagement'),
    (CAST('C4E80855-70FF-4863-92B1-7EE266426DE4' AS UNIQUEIDENTIFIER), 'BusinessPlans.Delete', 'Delete business plans', 'BusinessPlanManagement'),
    
    -- System Administration
    (CAST('D1E80855-70FF-4863-92B1-7EE266426DE1' AS UNIQUEIDENTIFIER), 'System.Admin', 'Full system administration', 'SystemAdministration'),
    (CAST('D2E80855-70FF-4863-92B1-7EE266426DE2' AS UNIQUEIDENTIFIER), 'System.Audit', 'View audit logs', 'SystemAdministration'),
    (CAST('D3E80855-70FF-4863-92B1-7EE266426DE3' AS UNIQUEIDENTIFIER), 'System.Settings', 'Manage system settings', 'SystemAdministration');

-- 3. Assign permissions to roles
-- Admin role gets all permissions (15 rows)
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    CAST('6FE80855-70FF-4863-92B1-7EE266426DEE' AS UNIQUEIDENTIFIER),
    [Id]
FROM [Permissions];

-- User role gets basic permissions (5 rows)
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    CAST('7FE80855-70FF-4863-92B1-7EE266426DEF' AS UNIQUEIDENTIFIER),
    [Id]
FROM [Permissions]
WHERE [Name] IN ('Users.Read', 'Organizations.Read', 'BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update');

-- OrganizationAdmin role gets organization and business plan permissions (9 rows)
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    CAST('8FE80855-70FF-4863-92B1-7EE266426DEA' AS UNIQUEIDENTIFIER),
    [Id]
FROM [Permissions]
WHERE [Name] IN (
    'Users.Read', 'Users.Update',
    'Organizations.Create', 'Organizations.Read', 'Organizations.Update',
    'BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update', 'BusinessPlans.Delete'
);

-- OrganizationMember role gets basic business plan permissions (3 rows)
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    CAST('9FE80855-70FF-4863-92B1-7EE266426DEB' AS UNIQUEIDENTIFIER),
    [Id]
FROM [Permissions]
WHERE [Name] IN ('BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update');

-- 4. Create a default organization (1 row)
INSERT INTO [Organizations] (
    [Id], [Name], [Description], [OrganizationType], [IsActive], 
    [MaxMembers], [AllowMemberInvites], [RequireEmailVerification],
    [Created], [CreatedBy], [IsDeleted]
)
VALUES (
    NEWID(),
    'Sqordia Default Organization',
    'Default organization for system administration',
    'Company',
    1,
    100,
    1,
    1,
    GETUTCDATE(),
    CAST('758afb07-3e8c-4259-995f-42f05af13b78' AS UNIQUEIDENTIFIER),
    0
);

PRINT 'Successfully executed parts completed!';
PRINT 'Created: 4 Roles, 15 Permissions, 32 Role-Permission mappings, 1 Organization';

