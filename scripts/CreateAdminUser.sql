-- Create Admin User with All Permissions
-- This script creates an admin user and assigns the Admin role
-- Run this after SeedAzureDatabase_Working.sql has been executed

-- 1. Create Admin User
INSERT INTO [Users] (
    [Id], 
    [FirstName], 
    [LastName], 
    [Email], 
    [UserName], 
    [PasswordHash], 
    [IsEmailConfirmed], 
    [EmailConfirmedAt], 
    [IsActive], 
    [UserType], 
    [AccessFailedCount], 
    [LockoutEnabled], 
    [LockoutEnd],
    [PhoneNumberVerified],
    [RequirePasswordChange],
    [Provider],
    [PasswordLastChangedAt],
    [Created], 
    [IsDeleted]
)
VALUES (
    CAST('758afb07-3e8c-4259-995f-42f05af13b78' AS UNIQUEIDENTIFIER),
    'Admin',
    'User',
    'admin@sqordia.com',
    'admin@sqordia.com',
    '$2a$11$mQQQUzX12zagdYn5YrbKN.PvvUZ8XHn7DsqjAvoYsXsVXJ6SSdKFa', -- Password: Sqordia2025!
    1, -- IsEmailConfirmed (true)
    GETUTCDATE(), -- EmailConfirmedAt
    1, -- IsActive (true)
    'Entrepreneur', -- UserType (valid values: Entrepreneur, OBNL, Consultant)
    0, -- AccessFailedCount
    1, -- LockoutEnabled (true)
    NULL, -- LockoutEnd (not locked)
    0, -- PhoneNumberVerified (false)
    0, -- RequirePasswordChange (false)
    'local', -- Provider
    GETUTCDATE(), -- PasswordLastChangedAt
    GETUTCDATE(), -- Created
    0 -- IsDeleted (false)
);

-- 2. Assign Admin Role to Admin User
INSERT INTO [UserRoles] ([Id], [UserId], [RoleId])
VALUES (
    NEWID(),
    CAST('758afb07-3e8c-4259-995f-42f05af13b78' AS UNIQUEIDENTIFIER),
    CAST('6FE80855-70FF-4863-92B1-7EE266426DEE' AS UNIQUEIDENTIFIER) -- Admin role
);

-- 3. Add Admin User to Default Organization (if organization exists)
INSERT INTO [OrganizationMembers] (
    [Id], 
    [OrganizationId], 
    [UserId], 
    [Role], 
    [IsActive], 
    [JoinedAt], 
    [Created], 
    [IsDeleted]
)
SELECT 
    NEWID(),
    o.[Id],
    CAST('758afb07-3e8c-4259-995f-42f05af13b78' AS UNIQUEIDENTIFIER),
    'Admin',
    1, -- IsActive
    GETUTCDATE(), -- JoinedAt
    GETUTCDATE(), -- Created
    0 -- IsDeleted
FROM [Organizations] o
WHERE o.[Name] = 'Sqordia Default Organization';

PRINT 'Admin user created successfully!';
PRINT 'Email: admin@sqordia.com';
PRINT 'Password: Sqordia2025!';
PRINT 'Role: Admin (with all permissions)';

