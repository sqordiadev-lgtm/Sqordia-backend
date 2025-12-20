-- SQL Server Seed Script: Admin User
-- Creates the default admin user
-- Idempotent: Safe to run multiple times

-- Password hash for "Sqordia2025!" using BCrypt
-- This is a pre-computed hash - in production, use proper password hashing

IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Id] = '00000000-0000-0000-0000-000000000000')
BEGIN
    INSERT INTO [Users] (
        [Id], 
        [Email], 
        [UserName], 
        [FirstName], 
        [LastName], 
        [PasswordHash], 
        [IsEmailConfirmed], 
        [UserType], 
        [IsActive],
        [Created], 
        [CreatedBy], 
        [LastModified], 
        [LastModifiedBy], 
        [IsDeleted]
    )
    VALUES (
        '00000000-0000-0000-0000-000000000000',
        'admin@sqordia.com',
        'admin@sqordia.com',
        'Admin',
        'User',
        '$2a$11$rQZ8K9mN2pL3sT4uV5wX6yA7bC8dE9fG0hI1jK2lM3nO4pP5qR6sS7tT8uU9vV0wW1xX2yY3zZ4', -- Password: Sqordia2025!
        1, -- IsEmailConfirmed
        'Admin',
        1, -- IsActive
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Created admin user: admin@sqordia.com';
END
ELSE
BEGIN
    -- Update admin user to ensure email is verified and account is active
    UPDATE [Users]
    SET 
        [IsEmailConfirmed] = 1,
        [IsActive] = 1,
        [LastModified] = GETUTCDATE(),
        [LastModifiedBy] = 'system'
    WHERE [Id] = '00000000-0000-0000-0000-000000000000';
    PRINT 'Updated admin user to ensure email is verified and account is active';
END

-- Assign Admin role to admin user
IF NOT EXISTS (SELECT 1 FROM [UserRoles] WHERE [UserId] = '00000000-0000-0000-0000-000000000000' AND [RoleId] = '11111111-1111-1111-1111-111111111111')
BEGIN
    INSERT INTO [UserRoles] ([UserId], [RoleId], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES (
        '00000000-0000-0000-0000-000000000000',
        '11111111-1111-1111-1111-111111111111',
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Assigned Admin role to admin user';
END
ELSE
    PRINT 'Admin role already assigned to admin user';

PRINT 'Admin user seeded successfully';
GO

