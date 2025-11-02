-- Seed Azure SQL Database with roles, permissions, and admin user
-- This script sets up the complete Sqordia database structure

-- 1. Create Roles
INSERT INTO [Roles] ([Id], [Name], [Description], [IsSystemRole])
VALUES 
    ('6FE80855-70FF-4863-92B1-7EE266426DEE', 'Admin', 'System Administrator with full access', 1),
    ('7FE80855-70FF-4863-92B1-7EE266426DEF', 'User', 'Regular user with basic access', 1),
    ('8FE80855-70FF-4863-92B1-7EE266426DEG', 'OrganizationAdmin', 'Organization Administrator', 0),
    ('9FE80855-70FF-4863-92B1-7EE266426DEH', 'OrganizationMember', 'Organization Member', 0);

-- 2. Create Permissions
INSERT INTO [Permissions] ([Id], [Name], [Description], [Category])
VALUES 
    -- User Management
    ('A1E80855-70FF-4863-92B1-7EE266426DE1', 'Users.Create', 'Create new users', 'UserManagement'),
    ('A2E80855-70FF-4863-92B1-7EE266426DE2', 'Users.Read', 'View users', 'UserManagement'),
    ('A3E80855-70FF-4863-92B1-7EE266426DE3', 'Users.Update', 'Update users', 'UserManagement'),
    ('A4E80855-70FF-4863-92B1-7EE266426DE4', 'Users.Delete', 'Delete users', 'UserManagement'),
    
    -- Organization Management
    ('B1E80855-70FF-4863-92B1-7EE266426DE1', 'Organizations.Create', 'Create organizations', 'OrganizationManagement'),
    ('B2E80855-70FF-4863-92B1-7EE266426DE2', 'Organizations.Read', 'View organizations', 'OrganizationManagement'),
    ('B3E80855-70FF-4863-92B1-7EE266426DE3', 'Organizations.Update', 'Update organizations', 'OrganizationManagement'),
    ('B4E80855-70FF-4863-92B1-7EE266426DE4', 'Organizations.Delete', 'Delete organizations', 'OrganizationManagement'),
    
    -- Business Plan Management
    ('C1E80855-70FF-4863-92B1-7EE266426DE1', 'BusinessPlans.Create', 'Create business plans', 'BusinessPlanManagement'),
    ('C2E80855-70FF-4863-92B1-7EE266426DE2', 'BusinessPlans.Read', 'View business plans', 'BusinessPlanManagement'),
    ('C3E80855-70FF-4863-92B1-7EE266426DE3', 'BusinessPlans.Update', 'Update business plans', 'BusinessPlanManagement'),
    ('C4E80855-70FF-4863-92B1-7EE266426DE4', 'BusinessPlans.Delete', 'Delete business plans', 'BusinessPlanManagement'),
    
    -- System Administration
    ('D1E80855-70FF-4863-92B1-7EE266426DE1', 'System.Admin', 'Full system administration', 'SystemAdministration'),
    ('D2E80855-70FF-4863-92B1-7EE266426DE2', 'System.Audit', 'View audit logs', 'SystemAdministration'),
    ('D3E80855-70FF-4863-92B1-7EE266426DE3', 'System.Settings', 'Manage system settings', 'SystemAdministration');

-- 3. Assign permissions to roles
-- Admin role gets all permissions
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '6FE80855-70FF-4863-92B1-7EE266426DEE',
    [Id]
FROM [Permissions];

-- User role gets basic permissions
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '7FE80855-70FF-4863-92B1-7EE266426DEF',
    [Id]
FROM [Permissions]
WHERE [Name] IN ('Users.Read', 'Organizations.Read', 'BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update');

-- OrganizationAdmin role gets organization and business plan permissions
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '8FE80855-70FF-4863-92B1-7EE266426DEG',
    [Id]
FROM [Permissions]
WHERE [Name] IN (
    'Users.Read', 'Users.Update',
    'Organizations.Create', 'Organizations.Read', 'Organizations.Update',
    'BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update', 'BusinessPlans.Delete'
);

-- OrganizationMember role gets basic business plan permissions
INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionId])
SELECT 
    NEWID(),
    '9FE80855-70FF-4863-92B1-7EE266426DEH',
    [Id]
FROM [Permissions]
WHERE [Name] IN ('BusinessPlans.Create', 'BusinessPlans.Read', 'BusinessPlans.Update');

-- 4. Create Admin User
INSERT INTO [Users] (
    [Id], [FirstName], [LastName], [Email], [UserName], [PasswordHash], 
    [IsEmailConfirmed], [EmailConfirmedAt], [IsActive], [UserType], 
    [Created], [IsDeleted]
)
VALUES (
    '758afb07-3e8c-4259-995f-42f05af13b78',
    'Admin',
    'User',
    'admin@sqordia.com',
    'admin@sqordia.com',
    '$2a$11$mQQQUzX12zagdYn5YrbKN.PvvUZ8XHn7DsqjAvoYsXsVXJ6SSdKFa', -- Password: Sqordia2025!
    1,
    GETUTCDATE(),
    1,
    'Admin',
    GETUTCDATE(),
    0
);

-- 5. Assign Admin role to admin user
INSERT INTO [UserRoles] ([Id], [UserId], [RoleId])
VALUES (
    NEWID(),
    '758afb07-3e8c-4259-995f-42f05af13b78',
    '6FE80855-70FF-4863-92B1-7EE266426DEE'
);

-- 6. Create a default organization
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
    '758afb07-3e8c-4259-995f-42f05af13b78',
    0
);

-- 7. Add admin user to the organization as admin
INSERT INTO [OrganizationMembers] (
    [Id], [OrganizationId], [UserId], [Role], [IsActive], [JoinedAt], 
    [Created], [IsDeleted]
)
SELECT 
    NEWID(),
    o.[Id],
    '758afb07-3e8c-4259-995f-42f05af13b78',
    'Admin',
    1,
    GETUTCDATE(),
    GETUTCDATE(),
    0
FROM [Organizations] o
WHERE o.[Name] = 'Sqordia Default Organization';

PRINT 'Database seeding completed successfully!';
PRINT 'Admin user created: admin@sqordia.com';
PRINT 'Password: Sqordia2025!';
