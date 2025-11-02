# Set Azure Storage Connection String

The `AZURE_STORAGE_CONNECTION_STRING` is **optional** but recommended if your application needs file storage functionality (document uploads, file management, etc.).

## Option 1: Create a New Storage Account (Recommended)

### Step 1: Create the Storage Account

Run this command to create a storage account in your resource group:

```bash
az storage account create \
  --name sqordiastorage \
  --resource-group Sqordia-group \
  --location eastus \
  --sku Standard_LRS \
  --kind StorageV2
```

**Note**: 
- Storage account names must be globally unique (3-24 characters, lowercase letters and numbers only)
- If `sqordiastorage` is taken, try: `sqordiastoragedev`, `sqordiafilestore`, etc.
- Replace `eastus` with your preferred location (e.g., `westus`, `eastus2`, `westeurope`)

### Step 2: Get the Connection String

Once created, get the connection string:

```bash
az storage account show-connection-string \
  --name sqordiastorage \
  --resource-group Sqordia-group \
  --query connectionString \
  --output tsv
```

This will output something like:
```
DefaultEndpointsProtocol=https;AccountName=sqordiastorage;AccountKey=abc123...xyz;EndpointSuffix=core.windows.net
```

### Step 3: Create the Container

Create the `documents` container (if needed):

```bash
az storage container create \
  --name documents \
  --account-name sqordiastorage \
  --auth-mode login
```

### Step 4: Add to GitHub Secrets

1. Go to: `https://github.com/sqordiadev-lgtm/Sqordia-backend/settings/secrets/actions`
2. Click **"New repository secret"** (or update existing `AZURE_STORAGE_CONNECTION_STRING`)
3. **Name**: `AZURE_STORAGE_CONNECTION_STRING`
4. **Value**: Paste the entire connection string from Step 2
5. Click **"Add secret"**

---

## Option 2: Use an Existing Storage Account

If you already have an Azure Storage account:

### Step 1: Get the Connection String

```bash
# Replace 'your-storage-account-name' with your actual storage account name
az storage account show-connection-string \
  --name your-storage-account-name \
  --resource-group Sqordia-group \
  --query connectionString \
  --output tsv
```

### Step 2: Add to GitHub Secrets

Copy the connection string and add it as `AZURE_STORAGE_CONNECTION_STRING` in GitHub Secrets.

---

## Option 3: Get Connection String from Azure Portal

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to **Storage accounts**
3. Select your storage account (or create a new one)
4. Go to **Settings** → **Access keys**
5. Click **"Show"** next to **key1** or **key2**
6. Copy the **Connection string** value
7. Add it to GitHub Secrets as `AZURE_STORAGE_CONNECTION_STRING`

---

## Connection String Format

The connection string looks like:
```
DefaultEndpointsProtocol=https;AccountName=sqordiastorage;AccountKey=abc123def456ghi789jkl012mno345pqr678stu901vwx234yz;EndpointSuffix=core.windows.net
```

---

## Is This Required?

❌ **No, this is optional.**

The application will work without this secret, but file storage features will not function. If your application doesn't use file storage, you can skip this secret.

---

## Verify Setup

After adding the secret, the next GitHub Actions deployment will:
1. Set the `AZURE_STORAGE_CONNECTION_STRING` environment variable in Azure App Service
2. The application will use it for Azure Blob Storage operations

---

## Troubleshooting

### Error: "Storage account name not available"
- Storage account names must be globally unique
- Try a different name: `sqordiastorage2025`, `sqordiafilestore`, `sqordiastoragedev`, etc.

### Error: "Resource group not found"
- Make sure you're using the correct resource group: `Sqordia-group`
- Check with: `az group list --query "[].name" --output table`

### Container already exists
- If the `documents` container already exists, the create command will fail but that's OK
- The container is already ready to use

---

**Last Updated**: $(date)

