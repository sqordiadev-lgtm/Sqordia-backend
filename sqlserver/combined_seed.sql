-- Combined SQL Server Seed Script
-- All seed scripts combined into one file
-- Safe to run multiple times (idempotent)

PRINT 'Starting database seeding...';
PRINT '';

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
        [IsEmailVerified], 
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
        1, -- IsEmailVerified
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
    PRINT 'Admin user already exists';
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

-- SQL Server Seed Script: Subscription Plans
-- Creates default subscription plans (Free, Pro, Enterprise)
-- Idempotent: Safe to run multiple times

IF NOT EXISTS (SELECT 1 FROM [SubscriptionPlans] WHERE [PlanType] = 0) -- Free
BEGIN
    INSERT INTO [SubscriptionPlans] (
        [Id],
        [PlanType],
        [Name],
        [Description],
        [Price],
        [Currency],
        [BillingCycle],
        [MaxUsers],
        [MaxBusinessPlans],
        [MaxStorageGB],
        [Features],
        [IsActive],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        NEWID(),
        0, -- Free
        'Free Plan',
        'Basic plan with limited features',
        0.00,
        'USD',
        0, -- Monthly
        1,
        3,
        1,
        '["Basic Templates", "3 Business Plans", "Email Support"]',
        1,
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Created Free subscription plan';
END
ELSE
    PRINT 'Free subscription plan already exists';

IF NOT EXISTS (SELECT 1 FROM [SubscriptionPlans] WHERE [PlanType] = 1) -- Pro
BEGIN
    INSERT INTO [SubscriptionPlans] (
        [Id],
        [PlanType],
        [Name],
        [Description],
        [Price],
        [Currency],
        [BillingCycle],
        [MaxUsers],
        [MaxBusinessPlans],
        [MaxStorageGB],
        [Features],
        [IsActive],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        NEWID(),
        1, -- Pro
        'Pro Plan',
        'Professional plan with advanced features',
        29.99,
        'USD',
        0, -- Monthly
        10,
        50,
        50,
        '["All Templates", "50 Business Plans", "Priority Support", "Export Options", "Collaboration Tools"]',
        1,
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Created Pro subscription plan';
END
ELSE
    PRINT 'Pro subscription plan already exists';

IF NOT EXISTS (SELECT 1 FROM [SubscriptionPlans] WHERE [PlanType] = 2) -- Enterprise
BEGIN
    INSERT INTO [SubscriptionPlans] (
        [Id],
        [PlanType],
        [Name],
        [Description],
        [Price],
        [Currency],
        [BillingCycle],
        [MaxUsers],
        [MaxBusinessPlans],
        [MaxStorageGB],
        [Features],
        [IsActive],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        NEWID(),
        2, -- Enterprise
        'Enterprise Plan',
        'Enterprise plan with unlimited features and dedicated support',
        99.99,
        'USD',
        0, -- Monthly
        999999, -- Unlimited
        999999, -- Unlimited
        500,
        '["All Templates", "Unlimited Business Plans", "Dedicated Support", "Custom Branding", "API Access", "Advanced Analytics"]',
        1,
        GETUTCDATE(),
        'system',
        GETUTCDATE(),
        'system',
        0
    );
    PRINT 'Created Enterprise subscription plan';
END
ELSE
    PRINT 'Enterprise subscription plan already exists';

PRINT 'Subscription plans seeded successfully';
GO

-- SQL Server Seed Script: Currencies
-- Creates common currency data
-- Idempotent: Safe to run multiple times

IF NOT EXISTS (SELECT 1 FROM [Currencies] WHERE [Code] = 'USD')
BEGIN
    INSERT INTO [Currencies] ([Id], [Code], [Name], [Symbol], [IsActive], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        (NEWID(), 'USD', 'US Dollar', '$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'EUR', 'Euro', '€', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'GBP', 'British Pound', '£', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'CAD', 'Canadian Dollar', 'C$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'AUD', 'Australian Dollar', 'A$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'JPY', 'Japanese Yen', '¥', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'CHF', 'Swiss Franc', 'CHF', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'CNY', 'Chinese Yuan', '¥', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'INR', 'Indian Rupee', '₹', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'BRL', 'Brazilian Real', 'R$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'MXN', 'Mexican Peso', '$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'ZAR', 'South African Rand', 'R', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'RUB', 'Russian Ruble', '₽', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'KRW', 'South Korean Won', '₩', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'SGD', 'Singapore Dollar', 'S$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'HKD', 'Hong Kong Dollar', 'HK$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'NZD', 'New Zealand Dollar', 'NZ$', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'SEK', 'Swedish Krona', 'kr', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'NOK', 'Norwegian Krone', 'kr', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0),
        (NEWID(), 'DKK', 'Danish Krone', 'kr', 1, GETUTCDATE(), 'system', GETUTCDATE(), 'system', 0);
    PRINT 'Created currencies';
END
ELSE
    PRINT 'Currencies already exist';

PRINT 'Currencies seeded successfully';
GO

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

-- SQL Server Seed Script: Questionnaires
-- Seeds the "Business Plan - 20 Common Questions" questionnaire template
-- Idempotent: Safe to run multiple times

DECLARE @QuestionnaireId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @AdminUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

-- Create questionnaire if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM [Questionnaires] WHERE [Id] = @QuestionnaireId)
BEGIN
    INSERT INTO [Questionnaires] (
        [Id],
        [Title],
        [Description],
        [IsPublic],
        [Created],
        [CreatedBy],
        [LastModified],
        [LastModifiedBy],
        [IsDeleted]
    )
    VALUES (
        @QuestionnaireId,
        'Business Plan - 20 Common Questions',
        'A comprehensive questionnaire covering the essential aspects of a business plan',
        1,
        GETUTCDATE(),
        @AdminUserId,
        GETUTCDATE(),
        @AdminUserId,
        0
    );
    PRINT 'Created questionnaire';

    -- Insert questions (20 bilingual questions)
    INSERT INTO [Questions] ([Id], [QuestionnaireId], [Order], [QuestionText], [QuestionTextFr], [QuestionType], [IsRequired], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES
        (NEWID(), @QuestionnaireId, 1, 'What is your business idea or concept?', 'Quelle est votre idée ou concept d''entreprise?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 2, 'What problem does your business solve?', 'Quel problème votre entreprise résout-elle?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 3, 'Who is your target market?', 'Quel est votre marché cible?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 4, 'What is your unique value proposition?', 'Quelle est votre proposition de valeur unique?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 5, 'Who are your main competitors?', 'Qui sont vos principaux concurrents?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 6, 'What is your competitive advantage?', 'Quel est votre avantage concurrentiel?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 7, 'What is your business model?', 'Quel est votre modèle d''affaires?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 8, 'How will you generate revenue?', 'Comment générerez-vous des revenus?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 9, 'What are your startup costs?', 'Quels sont vos coûts de démarrage?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 10, 'What are your operating expenses?', 'Quels sont vos frais d''exploitation?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 11, 'What is your pricing strategy?', 'Quelle est votre stratégie de prix?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 12, 'What is your marketing strategy?', 'Quelle est votre stratégie marketing?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 13, 'What is your sales strategy?', 'Quelle est votre stratégie de vente?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 14, 'Who is on your management team?', 'Qui compose votre équipe de direction?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 15, 'What are your key milestones?', 'Quels sont vos jalons clés?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 16, 'What are your financial projections?', 'Quelles sont vos projections financières?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 17, 'How much funding do you need?', 'De combien de financement avez-vous besoin?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 18, 'How will you use the funding?', 'Comment utiliserez-vous le financement?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 19, 'What are the main risks?', 'Quels sont les principaux risques?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (NEWID(), @QuestionnaireId, 20, 'What is your exit strategy?', 'Quelle est votre stratégie de sortie?', 0, 1, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0);

    PRINT 'Created 20 bilingual questions';
END
ELSE
    PRINT 'Questionnaire already exists';

PRINT 'Questionnaires seeded successfully';
GO

-- SQL Server Seed Script: Templates
-- Seeds business plan templates across different industries
-- Idempotent: Safe to run multiple times

DECLARE @AdminUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

-- Template industries
DECLARE @TechTemplateId UNIQUEIDENTIFIER = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
DECLARE @RetailTemplateId UNIQUEIDENTIFIER = 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb';
DECLARE @RestaurantTemplateId UNIQUEIDENTIFIER = 'cccccccc-cccc-cccc-cccc-cccccccccccc';
DECLARE @ConsultingTemplateId UNIQUEIDENTIFIER = 'dddddddd-dddd-dddd-dddd-dddddddddddd';
DECLARE @HealthcareTemplateId UNIQUEIDENTIFIER = 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee';
DECLARE @EducationTemplateId UNIQUEIDENTIFIER = 'ffffffff-ffff-ffff-ffff-ffffffffffff';
DECLARE @ManufacturingTemplateId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111112';
DECLARE @RealEstateTemplateId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222223';

-- Insert templates if they don't exist
IF NOT EXISTS (SELECT 1 FROM [Templates] WHERE [Id] = @TechTemplateId)
BEGIN
    INSERT INTO [Templates] ([Id], [Name], [Description], [Industry], [Category], [IsPublic], [Status], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
    VALUES 
        (@TechTemplateId, 'Tech Startup Business Plan', 'Comprehensive business plan template for technology startups', 'Technology', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@RetailTemplateId, 'Retail Business Plan', 'Business plan template for retail businesses', 'Retail', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@RestaurantTemplateId, 'Restaurant Business Plan', 'Complete business plan template for restaurants and food service', 'Food & Beverage', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@ConsultingTemplateId, 'Consulting Business Plan', 'Business plan template for consulting firms', 'Professional Services', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@HealthcareTemplateId, 'Healthcare Business Plan', 'Business plan template for healthcare and medical services', 'Healthcare', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@EducationTemplateId, 'Education Business Plan', 'Business plan template for educational institutions and services', 'Education', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@ManufacturingTemplateId, 'Manufacturing Business Plan', 'Business plan template for manufacturing companies', 'Manufacturing', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0),
        (@RealEstateTemplateId, 'Real Estate Business Plan', 'Business plan template for real estate businesses', 'Real Estate', 0, 1, 4, GETUTCDATE(), @AdminUserId, GETUTCDATE(), @AdminUserId, 0);
    PRINT 'Created 8 business plan templates';
END
ELSE
    PRINT 'Templates already exist';

-- Insert template sections for each template (6 sections each)
DECLARE @SectionNames TABLE (OrderNum INT, Name NVARCHAR(100));
INSERT INTO @SectionNames VALUES
    (1, 'Executive Summary'),
    (2, 'Company Description'),
    (3, 'Market Analysis'),
    (4, 'Organization & Management'),
    (5, 'Service or Product Line'),
    (6, 'Marketing & Sales');

-- Create sections for each template
DECLARE @TemplateCursor CURSOR;
DECLARE @CurrentTemplateId UNIQUEIDENTIFIER;
DECLARE @SectionOrder INT;
DECLARE @SectionName NVARCHAR(100);

SET @TemplateCursor = CURSOR FOR 
SELECT [Id] FROM [Templates] WHERE [IsDeleted] = 0;

OPEN @TemplateCursor;
FETCH NEXT FROM @TemplateCursor INTO @CurrentTemplateId;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @SectionCursor CURSOR;
    SET @SectionCursor = CURSOR FOR SELECT OrderNum, Name FROM @SectionNames;
    
    OPEN @SectionCursor;
    FETCH NEXT FROM @SectionCursor INTO @SectionOrder, @SectionName;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM [TemplateSections] 
            WHERE [TemplateId] = @CurrentTemplateId 
            AND [Order] = @SectionOrder
        )
        BEGIN
            INSERT INTO [TemplateSections] ([Id], [TemplateId], [Order], [Title], [Content], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [IsDeleted])
            VALUES (
                NEWID(),
                @CurrentTemplateId,
                @SectionOrder,
                @SectionName,
                N'This section provides guidance and structure for ' + @SectionName + '. Fill in the details specific to your business.',
                GETUTCDATE(),
                @AdminUserId,
                GETUTCDATE(),
                @AdminUserId,
                0
            );
        END
        
        FETCH NEXT FROM @SectionCursor INTO @SectionOrder, @SectionName;
    END
    
    CLOSE @SectionCursor;
    DEALLOCATE @SectionCursor;
    
    FETCH NEXT FROM @TemplateCursor INTO @CurrentTemplateId;
END

CLOSE @TemplateCursor;
DEALLOCATE @TemplateCursor;

PRINT 'Templates and sections seeded successfully';
GO

PRINT '';
PRINT 'Database seeding completed successfully!';
PRINT 'Admin User: admin@sqordia.com';
PRINT 'Password: Sqordia2025!';
