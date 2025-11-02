# Sqordia API Postman Collection

This directory contains Postman collections and environments for testing the Sqordia API.

## 📁 Files Overview

### Collections
- **`Sqordia_API_Collection.json`** - Latest comprehensive collection (v8.0.0) with all endpoints

### Environments
- **`Dev.json`** - Local development environment (port 5241) - **Recommended for local testing**
- **`Prod.json`** - Production environment (Azure) - **https://sqordia-backend-api.azurewebsites.net**
- **`Azure.json`** - Azure production environment (legacy)
- **`Local.json`** - Basic local development environment

## 🚀 Quick Start

### 1. Import Collection and Environment

1. **Import Collection**: Import `Sqordia_API_Collection.json` into Postman
2. **Import Environment**: Import `Dev.json` into Postman (for local testing)
3. **Select Environment**: Choose "Sqordia Dev Environment" in Postman

### 2. Test the API

#### Step 1: Health Check
```
GET {{base_url}}/health
```
- **Expected**: 200 OK with health status

#### Step 2: Seed Database (Admin Only)
```
POST {{base_url}}/api/v1/seed/database
```
- **Expected**: 200 OK with seeding confirmation
- **Note**: This creates admin user and initial data

#### Step 3: Login as Admin
```
POST {{base_url}}/api/v1/auth/login
Body: {
  "email": "admin@sqordia.com",
  "password": "Sqordia2025!"
}
```
- **Expected**: 200 OK with JWT token
- **Auto-populates**: `jwt_token`, `refresh_token`, `user_id`

#### Step 4: Test Authenticated Endpoints
```
GET {{base_url}}/api/v1/auth/me
```
- **Expected**: 200 OK with user details

## 🔧 Environment Variables

### Development Environment Variables
| Variable | Value | Description |
|----------|-------|-------------|
| `base_url` | `http://localhost:5241` | Local development URL (port 5241) |
| `admin_email` | `admin@sqordia.com` | Admin email for testing |
| `admin_password` | `Sqordia2025!` | Admin password |
| `jwt_token` | (auto-populated) | JWT authentication token |
| `refresh_token` | (auto-populated) | Refresh token |
| `user_id` | (auto-populated) | Current user ID |
| `organization_id` | (auto-populated) | Organization ID |
| `business_plan_id` | (auto-populated) | Business Plan ID |

## 📋 API Endpoints Overview

### 0. Health & System
- **Health Check** - Check API status
- **Get API Info** - Get API version info

### 1. Database Seeding (Admin Only)
- **Seed Database** - Initialize database with roles, permissions, admin user
- **Get Seed Status** - Check if database is seeded
- **Get Configuration** - Get database configuration

### 2. Authentication
- **Register User** - Create new user account
- **Login (Admin)** - Login with admin credentials
- **Login (Custom)** - Login with custom credentials
- **Get Current User** - Get current user info
- **Refresh Token** - Refresh JWT token
- **Logout** - Logout user
- **Revoke Token** - Revoke refresh token

### 3. User Profile
- **Get Profile** - Get user profile
- **Update Profile** - Update user profile
- **Change Password** - Change user password
- **Delete Account** - Delete user account

### 4. Two-Factor Authentication
- **Setup 2FA** - Get QR code for 2FA setup
- **Enable 2FA** - Enable 2FA with verification code
- **Disable 2FA** - Disable 2FA
- **Get 2FA Status** - Check 2FA status

### 5. Security & Sessions
- **Get Active Sessions** - List active user sessions
- **Revoke Specific Session** - Revoke specific session
- **Revoke All Other Sessions** - Revoke all other sessions
- **Revoke All Sessions** - Revoke all sessions
- **Get Login History** - Get user login history

### 6. Admin Dashboard
- **Get System Overview** - Get system metrics and overview
- **Get Users (Admin)** - Get paginated user list with filters

### 7. Role Management (Admin Only)
- **Get All Roles** - List all roles
- **Get Role By ID** - Get specific role
- **Create Role** - Create new role
- **Update Role** - Update existing role
- **Delete Role** - Delete role
- **Assign Role to User** - Assign role to user
- **Remove Role from User** - Remove role from user
- **Get User Roles** - Get user's roles
- **Get All Permissions** - List all permissions

### 8. Organizations
- **Create Organization** - Create new organization
- **Get User Organizations** - Get user's organizations
- **Get Organization** - Get organization details
- **Get Organization Detail** - Get organization with members
- **Update Organization** - Update organization
- **Delete Organization** - Delete organization
- **Get Organization Members** - Get organization members
- **Add Member** - Add member to organization
- **Update Member Role** - Update member role
- **Remove Member** - Remove member from organization

### 9. Business Plans
- **Create Business Plan** - Create new business plan
- **Get Business Plan** - Get business plan details
- **Get Organization Business Plans** - Get organization's business plans
- **Update Business Plan** - Update business plan
- **Delete Business Plan** - Delete business plan

### 10. Questionnaires
- **Get Questionnaire** - Get questionnaire for business plan
- **Submit Response** - Submit questionnaire response
- **Get Responses** - Get questionnaire responses

### 11. Business Plan Generation
- **Generate Business Plan** - Generate business plan using AI
- **Get Generation Status** - Check generation status

### 12. Financial Projections
- **Create Financial Projection** - Create financial projection
- **Get Financial Projections** - Get financial projections

### 13. Export & Reports
- **Export Business Plan (PDF)** - Export as PDF
- **Export Business Plan (Word)** - Export as Word document
- **Export Financial Report (Excel)** - Export financial data as Excel

## 🔐 Authentication

The collection uses Bearer token authentication. After login, the JWT token is automatically stored in the `jwt_token` variable and used for authenticated requests.

### Auto-Population
The collection automatically populates these variables after successful login:
- `jwt_token` - JWT authentication token
- `refresh_token` - Refresh token for token renewal
- `user_id` - Current user ID

### Auto-Population for Resources
These variables are populated when creating resources:
- `organization_id` - When creating organizations
- `business_plan_id` - When creating business plans

## 🌐 Environment Setup

### Production Environment (Prod.json)
- **URL**: `https://sqordia-backend-api.azurewebsites.net`
- **Admin**: `admin@sqordia.com` / `Sqordia2025!`
- **Status**: ✅ Deployed and ready

### Development Environment (Dev.json - Recommended for Testing)
- **URL**: `http://localhost:5241`
- **Admin**: `admin@sqordia.com` / `Sqordia2025!`
- **Status**: ✅ Ready for local development

## 🧪 Testing Workflow

### 1. Initial Setup
1. **Health Check** - Verify API is running
2. **Seed Database** - Initialize database (admin only)
3. **Login as Admin** - Get authentication token

### 2. User Management
1. **Register User** - Create test user
2. **Login as User** - Test user authentication
3. **Update Profile** - Test profile management

### 3. Organization Management
1. **Create Organization** - Create test organization
2. **Add Members** - Add users to organization
3. **Update Organization** - Test organization updates

### 4. Business Plan Workflow
1. **Create Business Plan** - Create test business plan
2. **Complete Questionnaire** - Submit questionnaire responses
3. **Generate Business Plan** - Use AI generation
4. **Export Business Plan** - Test export functionality

## 🔍 Troubleshooting

### Common Issues

#### 1. Authentication Errors
- **401 Unauthorized**: Check if JWT token is valid
- **403 Forbidden**: Check user permissions/roles
- **Solution**: Re-login to get fresh token

#### 2. Database Seeding Issues
- **Error**: "Database already seeded"
- **Solution**: Check seed status first, or use safe seeding script

#### 3. Missing Variables
- **Error**: Variables not populated
- **Solution**: Run login request first to populate authentication variables

#### 4. Azure Deployment Issues
- **Error**: Connection timeout
- **Solution**: Check Azure App Service status and health endpoint

### Debug Steps
1. **Check Health**: Always start with health check
2. **Verify Environment**: Ensure correct environment is selected
3. **Check Variables**: Verify all required variables are set
4. **Review Logs**: Check Azure App Service logs for server errors

## 📚 Additional Resources

- **API Documentation**: Available at `/swagger` endpoint
- **Health Monitoring**: Use health check endpoint for monitoring
- **Database Status**: Use seed status endpoint to check database state

## 🚀 Deployment Status

- **Azure App Service**: ✅ Deployed
- **Database**: ✅ Azure SQL Database
- **Authentication**: ✅ JWT with refresh tokens
- **Admin User**: ✅ `admin@sqordia.com` / `Sqordia2025!`
- **API Version**: v1.0
- **Environment**: Production

---

**Ready to test!** 🎉 Import the collection and environment, then start with the health check endpoint.