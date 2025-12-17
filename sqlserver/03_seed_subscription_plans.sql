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

