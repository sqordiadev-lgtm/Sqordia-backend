# Database Connection String for Azure

## How It Works

Your application uses `appsettings.Production.json` which contains:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;User Id=${DB_USERNAME};Password=${DB_PASSWORD};"
  }
}
```

However, Azure App Service **does not automatically replace `${DB_USERNAME}` and `${DB_PASSWORD}` placeholders** in appsettings files.

## Solution: Set the Full Connection String

You have **two options**:

### Option 1: Set Connection String Directly (Recommended)

Add the full connection string to Azure App Service. The connection string format is:

```
Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=sqordia;Password=YOUR_ACTUAL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

**Replace `YOUR_ACTUAL_PASSWORD`** with the actual password you set for `DB_PASSWORD`.

**How to set it:**
1. **Via Azure CLI**:
   ```bash
   az webapp config connection-string set \
     --name sqordia-backend-api \
     --resource-group Sqordia-group \
     --connection-string-type SQLAzure \
     --settings DefaultConnection="Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=sqordia;Password=YOUR_ACTUAL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   ```

2. **Via GitHub Actions Workflow** (Update the workflow):
   The workflow should set `ConnectionStrings__DefaultConnection` instead of separate `DB_USERNAME` and `DB_PASSWORD`.

### Option 2: Use Environment Variables (Current Setup)

If you want to keep using `DB_USERNAME` and `DB_PASSWORD` separately, you need to:

1. **Set the connection string via environment variable override**:
   ```bash
   az webapp config appsettings set \
     --name sqordia-backend-api \
     --resource-group Sqordia-group \
     --settings \
     ConnectionStrings__DefaultConnection="Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=sqordia;Password=$DB_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" \
     DB_USERNAME="sqordia" \
     DB_PASSWORD="YOUR_ACTUAL_PASSWORD"
   ```

## Recommended: Update GitHub Actions Workflow

The workflow should construct and set the full connection string. Here's what needs to be in the workflow:

```yaml
- name: Configure App Settings
  run: |
    RESOURCE_GROUP=$(az webapp list --query "[?name=='${{ env.AZURE_WEBAPP_NAME }}'].resourceGroup" -o tsv)
    
    # Construct the connection string
    CONNECTION_STRING="Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=${{ secrets.DB_USERNAME }};Password=${{ secrets.DB_PASSWORD }};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    
    az webapp config appsettings set --name ${{ env.AZURE_WEBAPP_NAME }} --resource-group $RESOURCE_GROUP --settings \
      ConnectionStrings__DefaultConnection="$CONNECTION_STRING" \
      JWT_SECRET="${{ secrets.JWT_SECRET }}" \
      # ... other settings
```

## What Value Goes in GitHub Secrets?

**You DON'T need to add the full connection string to GitHub Secrets.**

Instead, you need to add these **two separate secrets**:
1. **`DB_USERNAME`**: `sqordia`
2. **`DB_PASSWORD`**: Your actual database password

The **GitHub Actions workflow will automatically construct the full connection string** from these secrets and set it in Azure App Service as `ConnectionStrings__DefaultConnection`.

**So in GitHub Secrets, you only need:**
- `DB_USERNAME` = `sqordia`
- `DB_PASSWORD` = (your actual password, e.g., `YourNewSecurePassword123!`)

**The workflow does the rest!** ✅

---

## Quick Reference: Final Connection String Format

Once you have your `DB_PASSWORD`:

```
Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=sqordia;Password=YOUR_ACTUAL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

**Important**: 
- Replace `YOUR_ACTUAL_PASSWORD` with your actual `DB_PASSWORD` value
- The connection string should be set in Azure App Service, NOT in GitHub Secrets
- GitHub Secrets only need: `DB_USERNAME` and `DB_PASSWORD` (separate values)

