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

