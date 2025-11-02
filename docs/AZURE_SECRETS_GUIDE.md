# Step-by-Step Guide: Getting Azure Secrets for GitHub Actions

## Prerequisites
- Azure CLI installed ✅
- Access to Azure Portal
- GitHub repository access

---

## Step 1: Login to Azure CLI

```bash
az login
```

This will open a browser for authentication. Once logged in, verify your account:

```bash
az account show
```

If you have multiple subscriptions, list them:

```bash
az account list --output table
```

Set your active subscription (if needed):

```bash
az account set --subscription "YOUR_SUBSCRIPTION_ID_OR_NAME"
```

---

## Step 2: Get Azure Resource Information

First, let's identify your App Service details:

```bash
# List your app services
az webapp list --query "[].{Name:name, ResourceGroup:resourceGroup, Location:location}" --output table
```

**Note down**:
- App Service Name: `sqordia-backend-api`
- Resource Group: (will be shown in the output)

If you know the resource group, you can also find it:

```bash
az webapp show --name sqordia-backend-api --query "{ResourceGroup:resourceGroup, Location:location}" --output table
```

---

## Step 3: Create Service Principal (AZURE_CREDENTIALS)

### Option A: Full Access (Recommended for CI/CD)

Replace `YOUR_RESOURCE_GROUP` with your actual resource group name:

```bash
# Get your subscription ID
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
echo "Subscription ID: $SUBSCRIPTION_ID"

# Create service principal with contributor role for the resource group
az ad sp create-for-rbac \
  --name "sqordia-github-actions" \
  --role contributor \
  --scopes /subscriptions/$SUBSCRIPTION_ID/resourceGroups/YOUR_RESOURCE_GROUP \
  --sdk-auth
```

**Important**: The output will be a JSON object. **Copy this entire JSON** - this is your `AZURE_CREDENTIALS` secret.

Example output:
```json
{
  "clientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "clientSecret": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "subscriptionId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "tenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

### Option B: More Restrictive (Better Security)

If you want to limit permissions to just the App Service:

```bash
# Get App Service resource ID
APP_SERVICE_ID=$(az webapp show --name sqordia-backend-api --resource-group YOUR_RESOURCE_GROUP --query id -o tsv)

# Create service principal with contributor role for just the app service
az ad sp create-for-rbac \
  --name "sqordia-github-actions" \
  --role contributor \
  --scopes $APP_SERVICE_ID \
  --sdk-auth
```

**Note**: If you get an error about the service principal already existing, you can either:
1. Use a different name: `sqordia-github-actions-2`
2. Delete the existing one: `az ad sp delete --id http://sqordia-github-actions`

---

## Step 4: Get Publish Profile (AZURE_WEBAPP_PUBLISH_PROFILE)

### Method 1: Using Azure CLI

```bash
az webapp deployment list-publishing-profiles \
  --name sqordia-backend-api \
  --resource-group YOUR_RESOURCE_GROUP \
  --xml
```

**Copy the entire XML output** - this is your `AZURE_WEBAPP_PUBLISH_PROFILE` secret.

### Method 2: Using Azure Portal

1. Go to https://portal.azure.com
2. Navigate to your App Service: `sqordia-backend-api`
3. Click **Get publish profile** (in the top menu)
4. A `.PublishSettings` file will download
5. Open the file and **copy all XML content**

---

## Step 5: Get Database Connection Details

If you need the database password:

```bash
# List SQL servers
az sql server list --query "[].{Name:name, ResourceGroup:resourceGroup, Location:location}" --output table

# Get database connection string (replace YOUR_SQL_SERVER and YOUR_DATABASE)
az sql db show-connection-string \
  --server YOUR_SQL_SERVER \
  --name YOUR_DATABASE \
  --client ado.net
```

For the database username and password, you'll need to:
1. Check Azure Portal → SQL Server → Security → SQL authentication
2. Or check your existing connection strings in App Service Configuration

---

## Step 6: Add Secrets to GitHub

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Add each secret:

| Secret Name | Value Source |
|------------|--------------|
| `AZURE_CREDENTIALS` | JSON from Step 3 |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | XML from Step 4 |
| `DB_USERNAME` | Usually `sqordia` |
| `DB_PASSWORD` | From Azure Portal or your admin |

---

## Troubleshooting

### Error: "Service principal already exists"
```bash
# List existing service principals
az ad sp list --display-name "sqordia-github-actions" --query "[].{AppId:appId, DisplayName:displayName}" --output table

# Delete and recreate (if needed)
az ad sp delete --id http://sqordia-github-actions
```

### Error: "Insufficient privileges"
- Ensure you're logged in with an account that has Owner or User Access Administrator role
- Check subscription permissions: `az role assignment list --assignee $(az account show --query user.name -o tsv)`

### Can't find App Service
```bash
# Search all app services
az webapp list --query "[].{Name:name, ResourceGroup:resourceGroup}" --output table
```

---

## Verification

After adding secrets, verify the workflow can authenticate:

```bash
# Test service principal login (in GitHub Actions, this happens automatically)
az login --service-principal \
  -u CLIENT_ID \
  -p CLIENT_SECRET \
  --tenant TENANT_ID
```

---

## Security Best Practices

1. ✅ **Use Resource Group scope** (not subscription-wide)
2. ✅ **Rotate secrets regularly** (every 90 days recommended)
3. ✅ **Use separate service principals** for different environments
4. ✅ **Monitor service principal usage** in Azure AD
5. ✅ **Never commit secrets** to the repository

---

**Ready?** Run the commands above with your actual resource group name!

