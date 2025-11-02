# Set Azure SQL Database Password

## Quick Setup

To set or reset your Azure SQL Database password for the GitHub Actions secret:

### Step 1: Generate a Secure Password

Use a strong password (at least 16 characters, with uppercase, lowercase, numbers, and special characters).

Example: `SqordiaProd2025!@#SecurePass`

### Step 2: Set the Password in Azure

Run this command (replace with your chosen password):

```bash
az sql server update \
  --name sqordia-server \
  --resource-group Sqordia-group \
  --admin-password 'YourNewSecurePassword123!'
```

### Step 3: Add to GitHub Secrets

1. Go to: `https://github.com/sqordiadev-lgtm/Sqordia-backend/settings/secrets/actions`
2. Click **"New repository secret"** (or update existing `DB_PASSWORD`)
3. **Name**: `DB_PASSWORD`
4. **Value**: The password you just set (e.g., `YourNewSecurePassword123!`)
5. Click **"Add secret"**

### Step 4: Verify Connection String

Your connection string should be:
```
Server=tcp:sqordia-server.database.windows.net,1433;Initial Catalog=sqordiadb;Persist Security Info=False;User ID=sqordia;Password=YourNewSecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

**Note**: Replace `YourNewSecurePassword123!` with your actual password.

---

## If You Already Know the Password

If you already have the password, simply:
1. Copy it
2. Add it to GitHub Secrets as `DB_PASSWORD`
3. Make sure it matches the password set in Azure SQL Server

---

## Test the Connection

After setting up, you can test the connection:

```bash
# Test connection string (will prompt for password)
az sql db show-connection-string \
  --server sqordia-server \
  --name sqordiadb \
  --client ado.net
```

---

## Security Best Practices

1. ✅ Use a strong, unique password (minimum 16 characters)
2. ✅ Store it only in GitHub Secrets (never commit to code)
3. ✅ Rotate the password periodically
4. ✅ Use different passwords for dev and production

