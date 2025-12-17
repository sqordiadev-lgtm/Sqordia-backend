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

