# Azure App Service Troubleshooting

## Current Issue: Health Endpoint Returns Error

The app is deployed but returning errors when accessing `/health`. 

## Root Cause

The app settings in Azure are **empty**. The workflow tries to set them, but the secrets may not be configured in GitHub.

## Solution

You need to configure these app settings in Azure App Service. You can either:

### Option 1: Set via GitHub Secrets (Recommended)

Make sure these secrets are configured in GitHub (Settings → Secrets and variables → Actions):

- ✅ `AZURE_CREDENTIALS` - Already set
- ❓ `DB_USERNAME` - Needs value (e.g., `sqordia`)
- ❓ `DB_PASSWORD` - Needs your Azure SQL Database password
- ❓ `JWT_SECRET` - Needs a secure random string (min 32 chars)
- ❓ `SENDGRID_API_KEY` - Needs your SendGrid API key
- ❓ `OPENAI_API_KEY` - Optional but recommended
- ❓ `GOOGLE_OAUTH_CLIENT_ID` - Optional
- ❓ `GOOGLE_OAUTH_CLIENT_SECRET` - Optional

Then run the workflow again - it will automatically set the app settings.

### Option 2: Set Manually via Azure Portal

1. Go to Azure Portal → App Service → `sqordia-backend-api`
2. Navigate to **Configuration** → **Application settings**
3. Add these settings:

| Setting Name | Value | Example |
|-------------|-------|---------|
| `DB_USERNAME` | Your SQL username | `sqordia` |
| `DB_PASSWORD` | Your SQL password | (your actual password) |
| `JWT_SECRET` | Random secure string | (generate with `openssl rand -base64 32`) |
| `SENDGRID_API_KEY` | Your SendGrid key | `SG.xxx...` |
| `OPENAI_API_KEY` | Your OpenAI key | `sk-xxx...` |
| `ASPNETCORE_ENVIRONMENT` | Environment | `Production` |

4. Click **Save** and restart the app

### Option 3: Set via Azure CLI

```bash
az webapp config appsettings set \
  --name sqordia-backend-api \
  --resource-group Sqordia-group \
  --settings \
    DB_USERNAME="sqordia" \
    DB_PASSWORD="YOUR_DB_PASSWORD" \
    JWT_SECRET="YOUR_JWT_SECRET" \
    SENDGRID_API_KEY="YOUR_SENDGRID_KEY" \
    ASPNETCORE_ENVIRONMENT="Production"
```

Then restart:
```bash
az webapp restart --name sqordia-backend-api --resource-group Sqordia-group
```

## Verify Fix

After setting the app settings and restarting:

```bash
curl https://sqordia-backend-api.azurewebsites.net/health
```

Should return:
```json
{"status":"Healthy","timestamp":"2025-11-02T...","checks":[]}
```

## Current App Status

- ✅ App Service: Running
- ✅ Startup Command: `dotnet WebAPI.dll` (configured)
- ✅ .NET Version: 8.0 (configured)
- ❌ App Settings: Missing/Empty (needs configuration)

