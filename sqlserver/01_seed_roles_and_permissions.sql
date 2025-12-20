-- SQL Server Seed Script: Roles and Permissions
-- Creates essential roles and permissions for the application
-- Idempotent: Safe to run multiple times

-- Create Roles (Roles table only has: Id, Name, Description, IsSystemRole)
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '11111111-1111-1111-1111-111111111111')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole])
    VALUES ('11111111-1111-1111-1111-111111111111', 'Admin', 'System administrator with full access', 1);
    PRINT 'Created Admin role';
END
ELSE
    PRINT 'Admin role already exists';

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '22222222-2222-2222-2222-222222222222')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole])
    VALUES ('22222222-2222-2222-2222-222222222222', 'Collaborateur', 'Standard user role with edit access', 1);
    PRINT 'Created Collaborateur role';
END
ELSE
    PRINT 'Collaborateur role already exists';

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '33333333-3333-3333-3333-333333333333')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole])
    VALUES ('33333333-3333-3333-3333-333333333333', 'Lecteur', 'Read-only user role', 1);
    PRINT 'Created Lecteur role';
END
ELSE
    PRINT 'Lecteur role already exists';

-- Create Permissions (Permissions table has: Id, Name, Description, Category)
IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa')
BEGIN
    INSERT INTO [Permissions] ([Id], [Name], [Description], [Category])
    VALUES 
        ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Users.Read', 'Read user information', 'Users'),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Users.Create', 'Create new users', 'Users'),
        ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Users.Update', 'Update user information', 'Users'),
        ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Users.Delete', 'Delete users', 'Users'),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'BusinessPlans.Read', 'Read business plans', 'BusinessPlans'),
        ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'BusinessPlans.Create', 'Create business plans', 'BusinessPlans'),
        ('11111111-1111-1111-1111-111111111112', 'BusinessPlans.Update', 'Update business plans', 'BusinessPlans'),
        ('22222222-2222-2222-2222-222222222223', 'BusinessPlans.Delete', 'Delete business plans', 'BusinessPlans'),
        ('33333333-3333-3333-3333-333333333334', 'Organizations.Read', 'Read organization information', 'Organizations'),
        ('44444444-4444-4444-4444-444444444445', 'Organizations.Create', 'Create organizations', 'Organizations'),
        ('55555555-5555-5555-5555-555555555556', 'Organizations.Update', 'Update organizations', 'Organizations'),
        ('66666666-6666-6666-6666-666666666667', 'Organizations.Delete', 'Delete organizations', 'Organizations'),
        ('77777777-7777-7777-7777-777777777778', 'Admin.Dashboard', 'Access admin dashboard', 'Admin'),
        ('88888888-8888-8888-8888-888888888889', 'Admin.Settings', 'Manage system settings', 'Admin');
    PRINT 'Created permissions';
END
ELSE
    PRINT 'Permissions already exist';

-- Assign all permissions to Admin role (RolePermissions table has: Id, RoleId, PermissionId)
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '11111111-1111-1111-1111-111111111111',
    [Id]
FROM [Permissions]
WHERE [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '11111111-1111-1111-1111-111111111111'
);

-- Assign basic permissions to Collaborateur role
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '22222222-2222-2222-2222-222222222222',
    [Id]
FROM [Permissions]
WHERE [Name] IN ('Users.Read', 'BusinessPlans.Read', 'BusinessPlans.Create', 'BusinessPlans.Update', 'Organizations.Read')
AND [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '22222222-2222-2222-2222-222222222222'
);

-- Assign read-only permissions to Lecteur role
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '33333333-3333-3333-3333-333333333333',
    [Id]
FROM [Permissions]
WHERE [Name] LIKE '%.Read'
AND [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '33333333-3333-3333-3333-333333333333'
);

PRINT 'Roles and permissions seeded successfully';
GO
