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

