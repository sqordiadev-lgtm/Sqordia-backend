-- SQL Server Seed Script: Templates
-- Seeds business plan templates across different industries
-- Idempotent: Safe to run multiple times
-- Updated to match current schema

DECLARE @AdminUserId UNIQUEIDENTIFIER = (SELECT TOP 1 [Id] FROM [Users] WHERE [Email] LIKE '%admin%' OR [UserType] = 'Admin' ORDER BY [Created] DESC);
IF @AdminUserId IS NULL
    SET @AdminUserId = '00000000-0000-0000-0000-000000000000';

-- Template IDs
DECLARE @TechTemplateId UNIQUEIDENTIFIER = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa';
DECLARE @RetailTemplateId UNIQUEIDENTIFIER = 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb';
DECLARE @RestaurantTemplateId UNIQUEIDENTIFIER = 'cccccccc-cccc-cccc-cccc-cccccccccccc';
DECLARE @ConsultingTemplateId UNIQUEIDENTIFIER = 'dddddddd-dddd-dddd-dddd-dddddddddddd';
DECLARE @HealthcareTemplateId UNIQUEIDENTIFIER = 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee';
DECLARE @EducationTemplateId UNIQUEIDENTIFIER = 'ffffffff-ffff-ffff-ffff-ffffffffffff';
DECLARE @ManufacturingTemplateId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111112';
DECLARE @RealEstateTemplateId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222223';

-- TemplateCategory enum: BusinessPlan = 1, FinancialProjection = 2, MarketingPlan = 3, etc.
-- TemplateType enum: Standard = 1, Premium = 2, Custom = 3, IndustrySpecific = 4, etc.
-- TemplateStatus enum: Draft = 1, Published = 2, Archived = 3, Active = 4

-- Insert templates if they don't exist
IF NOT EXISTS (SELECT 1 FROM [Templates] WHERE [Id] = @TechTemplateId)
BEGIN
    INSERT INTO [Templates] (
        [Id], 
        [Name], 
        [Description], 
        [Content],
        [Category], 
        [Type],
        [Status],
        [Industry], 
        [TargetAudience],
        [Language],
        [Country],
        [IsPublic], 
        [IsDefault],
        [UsageCount],
        [Rating],
        [RatingCount],
        [Tags],
        [PreviewImage],
        [Author],
        [AuthorEmail],
        [Version],
        [Changelog],
        [LastUsed],
        [CreatedAt],
        [UpdatedAt],
        [CreatedBy],
        [UpdatedBy]
    )
    VALUES 
        (@TechTemplateId, 
         'Tech Startup Business Plan', 
         'Comprehensive business plan template for technology startups. Includes sections for market analysis, product development, financial projections, and growth strategy.', 
         'This template provides a structured approach to creating a business plan for technology startups...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Technology', 
         'Tech Startups, SaaS Companies, Software Developers',
         'en',
         'US',
         1, -- IsPublic
         0, -- IsDefault
         0, -- UsageCount
         0.0, -- Rating
         0, -- RatingCount
         'technology,startup,saas,software',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@RetailTemplateId, 
         'Retail Business Plan', 
         'Business plan template for retail businesses including brick-and-mortar stores and e-commerce operations.', 
         'This template helps retail businesses create comprehensive plans...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Retail', 
         'Retail Stores, E-commerce, Merchants',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'retail,ecommerce,merchandise',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@RestaurantTemplateId, 
         'Restaurant Business Plan', 
         'Complete business plan template for restaurants, cafes, and food service businesses.', 
         'This template covers all aspects of restaurant operations...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Food & Beverage', 
         'Restaurants, Cafes, Food Trucks, Catering',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'restaurant,food,beverage,hospitality',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@ConsultingTemplateId, 
         'Consulting Business Plan', 
         'Business plan template for consulting firms and professional service providers.', 
         'This template is designed for consulting businesses...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Professional Services', 
         'Consultants, Advisors, Professional Services',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'consulting,professional,services',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@HealthcareTemplateId, 
         'Healthcare Business Plan', 
         'Business plan template for healthcare and medical services businesses.', 
         'This template addresses the unique needs of healthcare businesses...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Healthcare', 
         'Medical Practices, Clinics, Health Services',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'healthcare,medical,health',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@EducationTemplateId, 
         'Education Business Plan', 
         'Business plan template for educational institutions and training services.', 
         'This template helps educational businesses create effective plans...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Education', 
         'Schools, Training Centers, Educational Services',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'education,training,learning',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@ManufacturingTemplateId, 
         'Manufacturing Business Plan', 
         'Business plan template for manufacturing companies and production businesses.', 
         'This template covers manufacturing operations...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Manufacturing', 
         'Manufacturers, Production Companies',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'manufacturing,production,industrial',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId),
         
        (@RealEstateTemplateId, 
         'Real Estate Business Plan', 
         'Business plan template for real estate businesses, property management, and development companies.', 
         'This template addresses real estate business needs...',
         1, -- BusinessPlan
         4, -- IndustrySpecific
         2, -- Published
         'Real Estate', 
         'Real Estate Agents, Property Managers, Developers',
         'en',
         'US',
         1,
         0,
         0,
         0.0,
         0,
         'realestate,property,development',
         '',
         'Sqordia Team',
         'admin@sqordia.com',
         '1.0.0',
         'Initial version',
         GETUTCDATE(),
         GETUTCDATE(),
         GETUTCDATE(),
         @AdminUserId,
         @AdminUserId);
         
    PRINT 'Created 8 business plan templates';
END
ELSE
    PRINT 'Templates already exist';

PRINT 'Template seeding completed successfully';
GO
