-- SQL Server Seed Script: Roles and Permissions
-- Creates essential roles and permissions for the application
-- Idempotent: Safe to run multiple times

-- Create Roles
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '11111111-1111-1111-1111-111111111111')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        ('11111111-1111-1111-1111-111111111111', 'Admin', 'System administrator with full access', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0);
    PRINT 'Created Admin role';
END
ELSE
    PRINT 'Admin role already exists';

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '22222222-2222-2222-2222-222222222222')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        ('22222222-2222-2222-2222-222222222222', 'Collaborateur', 'Standard user role', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0);
    PRINT 'Created Collaborateur role';
END
ELSE
    PRINT 'Collaborateur role already exists';

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [Id] = '33333333-3333-3333-3333-333333333333')
BEGIN
    INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        ('33333333-3333-3333-3333-333333333333', 'Lecteur', 'Read-only user role', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0);
    PRINT 'Created Lecteur role';
END
ELSE
    PRINT 'Lecteur role already exists';

-- Create Permissions
IF NOT EXISTS (SELECT 1 FROM [Permissions] WHERE [Id] = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa')
BEGIN
    INSERT INTO [Permissions] ([Id], [Name], [Description], [Resource], [Action], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Users.Read', 'Read user information', 'Users', 'Read', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Users.Create', 'Create new users', 'Users', 'Create', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Users.Update', 'Update user information', 'Users', 'Update', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Users.Delete', 'Delete users', 'Users', 'Delete', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'BusinessPlans.Read', 'Read business plans', 'BusinessPlans', 'Read', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'BusinessPlans.Create', 'Create business plans', 'BusinessPlans', 'Create', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('11111111-1111-1111-1111-111111111112', 'BusinessPlans.Update', 'Update business plans', 'BusinessPlans', 'Update', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('22222222-2222-2222-2222-222222222223', 'BusinessPlans.Delete', 'Delete business plans', 'BusinessPlans', 'Delete', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('33333333-3333-3333-3333-333333333334', 'Organizations.Read', 'Read organization information', 'Organizations', 'Read', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('44444444-4444-4444-4444-444444444445', 'Organizations.Create', 'Create organizations', 'Organizations', 'Create', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('55555555-5555-5555-5555-555555555556', 'Organizations.Update', 'Update organizations', 'Organizations', 'Update', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('66666666-6666-6666-6666-666666666667', 'Organizations.Delete', 'Delete organizations', 'Organizations', 'Delete', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('77777777-7777-7777-7777-777777777778', 'Admin.Dashboard', 'Access admin dashboard', 'Admin', 'Read', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        ('88888888-8888-8888-8888-888888888889', 'Admin.Settings', 'Manage system settings', 'Admin', 'Update', GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0);
    PRINT 'Created permissions';
END
ELSE
    PRINT 'Permissions already exist';

-- Assign all permissions to Admin role
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
SELECT 
    NEWID(),
    '11111111-1111-1111-1111-111111111111',
    [Id],
    GETUTCDATE(),
    'system',
    GETUTCDATE(),
    'system',
    0
FROM [Permissions]
WHERE [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '11111111-1111-1111-1111-111111111111'
);

-- Assign basic permissions to Collaborateur role
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
SELECT 
    NEWID(),
    '22222222-2222-2222-2222-222222222222',
    [Id],
    GETUTCDATE(),
    'system',
    GETUTCDATE(),
    'system',
    0
FROM [Permissions]
WHERE [Name] IN ('Users.Read', 'BusinessPlans.Read', 'BusinessPlans.Create', 'BusinessPlans.Update', 'Organizations.Read')
AND [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '22222222-2222-2222-2222-222222222222'
);

-- Assign read-only permissions to Lecteur role
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
SELECT 
    NEWID(),
    '33333333-3333-3333-3333-333333333333',
    [Id],
    GETUTCDATE(),
    'system',
    GETUTCDATE(),
    'system',
    0
FROM [Permissions]
WHERE [Action] = 'Read'
AND [Id] NOT IN (
    SELECT [PermissionId] 
    FROM [RolePermissions] 
    WHERE [RoleId] = '33333333-3333-3333-3333-333333333333'
);

PRINT 'Roles and permissions seeded successfully';
GO

