# GitHub Secrets Setup Guide

This guide explains the GitHub secrets you need to configure for the Azure deployment workflow.

## Required GitHub Secrets

### 1. **AZURE_CREDENTIALS** (Required)
**Purpose**: Service principal credentials for Azure authentication

**How to create**:
```bash
# Login to Azure CLI
az login

# Create a service principal (replace with your subscription ID and resource group)
az ad sp create-for-rbac --name "sqordia-github-actions" \
  --role contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} \
  --sdk-auth
```

**Format**: JSON string containing:
```json
{
  "clientId": "...",
  "clientSecret": "...",
  "subscriptionId": "...",
  "tenantId": "..."
}
```

---

### 2. **AZURE_WEBAPP_PUBLISH_PROFILE** (Required)
**Purpose**: Publish profile for deploying to Azure App Service

**How to get**:
1. Go to Azure Portal → Your App Service → **Get publish profile**
2. Download the `.PublishSettings` file
3. Copy the entire content (XML format)

---

### 3. **DB_USERNAME** (Optional - if not hardcoded)
**Purpose**: Azure SQL Database username

**Value**: `sqordia` (or your database username)

---

### 4. **DB_PASSWORD** (Required - sensitive)
**Purpose**: Azure SQL Database password

**Value**: Your database password

---

### 5. **JWT_SECRET** (Required - sensitive)
**Purpose**: JWT token signing secret (minimum 32 characters)

**How to generate**:
```bash
# Generate a secure random key
openssl rand -base64 32
```

**Requirements**: 
- Minimum 32 characters
- Should be unique and kept secret
- Store securely

---

### 6. **OPENAI_API_KEY** (Optional)
**Purpose**: OpenAI API key for AI features

**How to get**: 
1. Sign up at https://platform.openai.com
2. Create an API key in your dashboard
3. Copy the key (starts with `sk-`)

---

### 7. **SENDGRID_API_KEY** (Required for email)
**Purpose**: SendGrid API key for email sending

**How to get**:
1. Sign up at https://sendgrid.com
2. Go to Settings → API Keys
3. Create an API key with "Full Access" or "Mail Send" permissions
4. Copy the key (starts with `SG.`)

---

### 8. **CLAUDE_API_KEY** (Optional)
**Purpose**: Anthropic Claude API key for AI features

**How to get**: 
1. Sign up at https://console.anthropic.com
2. Create an API key
3. Copy the key

---

### 9. **GEMINI_API_KEY** (Optional)
**Purpose**: Google Gemini API key for AI features

**How to get**:
1. Go to https://aistudio.google.com/apikey
2. Create an API key
3. Copy the key

---

### 10. **GOOGLE_OAUTH_CLIENT_ID** (Optional)
**Purpose**: Google OAuth client ID for authentication

**How to get**:
1. Go to Google Cloud Console
2. Create OAuth 2.0 credentials
3. Copy the Client ID

---

### 11. **GOOGLE_OAUTH_CLIENT_SECRET** (Optional - sensitive)
**Purpose**: Google OAuth client secret

**How to get**:
1. Same as above, copy the Client Secret

---

### 12. **GOOGLE_OAUTH_REDIRECT_URI** (Optional)
**Purpose**: OAuth redirect URI

**For Production**:
- **Value**: `https://sqordia-backend-api.azurewebsites.net/api/v1/auth/google/callback`

**For Localhost Development**:
- **HTTP**: `http://localhost:5241/api/v1/auth/google/callback`
- **HTTPS**: `https://localhost:7148/api/v1/auth/google/callback`

**Note**: You need to configure both redirect URIs in your Google Cloud Console OAuth credentials to support both local development and production.

---

### 13. **AZURE_STORAGE_CONNECTION_STRING** (Optional)
**Purpose**: Azure Blob Storage connection string for file storage

**How to get**:
1. Azure Portal → Storage Account → Access Keys
2. Copy the connection string

---

## How to Add Secrets to GitHub

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Enter the secret name and value
5. Click **Add secret**

## Recommended Workflow Update

Instead of hardcoding sensitive values in the workflow, use GitHub secrets:

```yaml
az webapp config appsettings set --name ${{ env.AZURE_WEBAPP_NAME }} --resource-group $RESOURCE_GROUP --settings \
  DB_USERNAME="${{ secrets.DB_USERNAME }}" \
  DB_PASSWORD="${{ secrets.DB_PASSWORD }}" \
  JWT_SECRET="${{ secrets.JWT_SECRET }}" \
  OPENAI_API_KEY="${{ secrets.OPENAI_API_KEY }}" \
  SENDGRID_API_KEY="${{ secrets.SENDGRID_API_KEY }}" \
  SENDGRID_FROM_EMAIL="noreply@sqordia.com" \
  SENDGRID_FROM_NAME="Sqordia Team" \
  GOOGLE_OAUTH_CLIENT_ID="${{ secrets.GOOGLE_OAUTH_CLIENT_ID }}" \
  GOOGLE_OAUTH_CLIENT_SECRET="${{ secrets.GOOGLE_OAUTH_CLIENT_SECRET }}" \
  GOOGLE_OAUTH_REDIRECT_URI="https://sqordia-backend-api.azurewebsites.net/api/v1/auth/google/callback" \
  AZURE_STORAGE_CONNECTION_STRING="${{ secrets.AZURE_STORAGE_CONNECTION_STRING }}"
```

## Security Best Practices

1. ✅ **Never commit secrets** to the repository
2. ✅ **Use GitHub Secrets** for all sensitive values
3. ✅ **Rotate secrets regularly** (especially JWT_SECRET and passwords)
4. ✅ **Use least privilege** for service principals
5. ✅ **Review secret access** regularly in GitHub settings

## Minimum Required Secrets

For basic deployment, you need:
- ✅ `AZURE_CREDENTIALS`
- ✅ `AZURE_WEBAPP_PUBLISH_PROFILE`
- ✅ `DB_PASSWORD`
- ✅ `JWT_SECRET`
- ✅ `SENDGRID_API_KEY` (required for email functionality)

Optional but recommended:
- `OPENAI_API_KEY` (for AI features)
- `GOOGLE_OAUTH_CLIENT_ID` & `GOOGLE_OAUTH_CLIENT_SECRET` (for OAuth)
- `AZURE_STORAGE_CONNECTION_STRING` (for file storage)

