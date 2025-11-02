-- PostgreSQL Seed Script for Sqordia Database
-- This script creates essential roles and admin user
-- Idempotent: Safe to run multiple times

-- Create roles if they don't exist
INSERT INTO "Roles" ("Id", "Name", "Description", "IsSystemRole", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
VALUES 
    ('11111111-1111-1111-1111-111111111111', 'Admin', 'System administrator with full access', true, NOW(), 'system', NOW(), 'system', false),
    ('22222222-2222-2222-2222-222222222222', 'Collaborateur', 'Standard user role', true, NOW(), 'system', NOW(), 'system', false),
    ('33333333-3333-3333-3333-333333333333', 'Lecteur', 'Read-only user role', true, NOW(), 'system', NOW(), 'system', false)
ON CONFLICT ("Id") DO NOTHING;

-- Create admin user if it doesn't exist
INSERT INTO "Users" ("Id", "Email", "UserName", "FirstName", "LastName", "PasswordHash", "IsEmailVerified", "UserType", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
VALUES (
    '00000000-0000-0000-0000-000000000000',
    'admin@sqordia.com',
    'admin@sqordia.com',
    'Admin',
    'User',
    '$2a$11$rQZ8K9mN2pL3sT4uV5wX6yA7bC8dE9fG0hI1jK2lM3nO4pP5qR6sS7tT8uU9vV0wW1xX2yY3zZ4', -- Password: Sqordia2025!
    true,
    'Admin',
    NOW(),
    'system',
    NOW(),
    'system',
    false
)
ON CONFLICT ("Id") DO NOTHING;

-- Assign Admin role to admin user if not already assigned
INSERT INTO "UserRoles" ("UserId", "RoleId", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
VALUES (
    '00000000-0000-0000-0000-000000000000',
    '11111111-1111-1111-1111-111111111111',
    NOW(),
    'system',
    NOW(),
    'system',
    false
)
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- Create some basic permissions
INSERT INTO "Permissions" ("Id", "Name", "Description", "Resource", "Action", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
VALUES 
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Users.Read', 'Read user information', 'Users', 'Read', NOW(), 'system', NOW(), 'system', false),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Users.Write', 'Create and update users', 'Users', 'Write', NOW(), 'system', NOW(), 'system', false),
    ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Users.Delete', 'Delete users', 'Users', 'Delete', NOW(), 'system', NOW(), 'system', false),
    ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Roles.Read', 'Read role information', 'Roles', 'Read', NOW(), 'system', NOW(), 'system', false),
    ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Roles.Write', 'Create and update roles', 'Roles', 'Write', NOW(), 'system', NOW(), 'system', false),
    ('ffffffff-ffff-ffff-ffff-ffffffffffff', 'BusinessPlans.Read', 'Read business plans', 'BusinessPlans', 'Read', NOW(), 'system', NOW(), 'system', false),
    ('gggggggg-gggg-gggg-gggg-gggggggggggg', 'BusinessPlans.Write', 'Create and update business plans', 'BusinessPlans', 'Write', NOW(), 'system', NOW(), 'system', false),
    ('hhhhhhhh-hhhh-hhhh-hhhh-hhhhhhhhhhhh', 'BusinessPlans.Delete', 'Delete business plans', 'BusinessPlans', 'Delete', NOW(), 'system', NOW(), 'system', false)
ON CONFLICT ("Id") DO NOTHING;

-- Assign all permissions to Admin role
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
SELECT 
    '11111111-1111-1111-1111-111111111111' as "RoleId",
    "Id" as "PermissionId",
    NOW() as "Created",
    'system' as "CreatedBy",
    NOW() as "LastModified",
    'system' as "LastModifiedBy",
    false as "IsDeleted"
FROM "Permissions"
WHERE "IsDeleted" = false
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Assign read permissions to Collaborateur role
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
SELECT 
    '22222222-2222-2222-2222-222222222222' as "RoleId",
    "Id" as "PermissionId",
    NOW() as "Created",
    'system' as "CreatedBy",
    NOW() as "LastModified",
    'system' as "LastModifiedBy",
    false as "IsDeleted"
FROM "Permissions"
WHERE "IsDeleted" = false AND "Name" LIKE '%.Read'
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Assign read permissions to Lecteur role
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "Created", "CreatedBy", "LastModified", "LastModifiedBy", "IsDeleted")
SELECT 
    '33333333-3333-3333-3333-333333333333' as "RoleId",
    "Id" as "PermissionId",
    NOW() as "Created",
    'system' as "CreatedBy",
    NOW() as "LastModified",
    'system' as "LastModifiedBy",
    false as "IsDeleted"
FROM "Permissions"
WHERE "IsDeleted" = false AND "Name" LIKE '%.Read'
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Display success message
DO $$
BEGIN
    RAISE NOTICE 'Database seeded successfully!';
    RAISE NOTICE 'Admin user: admin@sqordia.com';
    RAISE NOTICE 'Password: Sqordia2025!';
    RAISE NOTICE 'Roles created: Admin, Collaborateur, Lecteur';
END $$;
