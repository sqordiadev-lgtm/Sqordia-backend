-- SQL Server Seed Script: Default Organization
-- Creates a default organization and assigns the admin user to it
-- Idempotent: Safe to run multiple times

DECLARE @OrganizationId UNIQUEIDENTIFIER = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
DECLARE @AdminUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

-- Create default organization
IF NOT EXISTS (SELECT 1 FROM [Organizations] WHERE [Id] = @OrganizationId)
BEGIN
    INSERT INTO [Organizations] (
        [Id],
        [Name],
        [Description],
        [OrganizationType],
        [IsActive],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        @OrganizationId,
        'Sqordia Default Organization',
        'Default organization for system administration and testing',
        0, -- Startup
        1,
        GETUTCDATE(),
        @AdminUserId,
        GETUTCDATE(),
        @AdminUserId,
        0
    );
    PRINT 'Created default organization';
END
ELSE
    PRINT 'Default organization already exists';

-- Assign admin user to the organization
IF NOT EXISTS (
    SELECT 1 
    FROM [OrganizationMembers] 
    WHERE [OrganizationId] = @OrganizationId 
    AND [UserId] = @AdminUserId
)
BEGIN
    INSERT INTO [OrganizationMembers] (
        [Id],
        [OrganizationId],
        [UserId],
        [Role],
        [IsActive],
        [JoinedAt],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        NEWID(),
        @OrganizationId,
        @AdminUserId,
        0, -- Admin
        1,
        GETUTCDATE(),
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Assigned admin user to default organization';
END
ELSE
    PRINT 'Admin user already assigned to default organization';

PRINT 'Default organization seeded successfully';
GO

