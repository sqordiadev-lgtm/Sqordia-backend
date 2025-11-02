# Secrets Configuration Guide

This guide explains how to configure application secrets for local development.

## Quick Setup

### Option 1: Using secrets.json (Recommended for Local Dev)

1. **Copy the template:**
   ```bash
   cp secrets.json.template secrets.json
   ```

2. **Edit `secrets.json` with your actual values:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=SqordiaDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;MultipleActiveResultSets=true;"
     },
     "JwtSettings": {
       "Secret": "YOUR_32_CHAR_SECRET"
     },
     "SendGrid": {
       "ApiKey": "YOUR_SENDGRID_KEY",
       "FromEmail": "noreply@sqordia.com",
       "FromName": "Sqordia"
     }
   }
   ```

3. **Load secrets when running the app:**
   - The app will automatically read from `secrets.json` if you configure it
   - **Note:** You need to update `Program.cs` to load this file (see below)

### Option 2: Using .NET User Secrets (Current Method)

This is the current method being used. Secrets are stored securely outside the project directory.

```bash
cd src/WebAPI

# Set database connection
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=SqordiaDb;User Id=sa;Password=SqlServer123!;TrustServerCertificate=true;MultipleActiveResultSets=true;"

# Set JWT secret
dotnet user-secrets set "JwtSettings:Secret" "S6075c41562a35879f7e40220d435f1f6972c7c7a9deea0c5"

# Set SendGrid API key
dotnet user-secrets set "SendGrid:ApiKey" "YOUR_SENDGRID_API_KEY"

# List all secrets
dotnet user-secrets list
```

## Required Secrets

### 1. Database Connection String
- **Key:** `ConnectionStrings:DefaultConnection`
- **Value:** SQL Server connection string
- **Example:** `Server=localhost,1433;Database=SqordiaDb;User Id=sa;Password=SqlServer123!;TrustServerCertificate=true;MultipleActiveResultSets=true;`

### 2. JWT Secret
- **Key:** `JwtSettings:Secret`
- **Value:** Secure random string (at least 32 characters)
- **Example:** `S6075c41562a35879f7e40220d435f1f6972c7c7a9deea0c5`
- **Generate new secret:**
  ```bash
  openssl rand -hex 32
  ```

### 3. SendGrid API Key (Optional for Email)
- **Key:** `SendGrid:ApiKey`
- **Value:** Your SendGrid API key from https://app.sendgrid.com/settings/api_keys
- **Example:** `SG.xxxxxxxxxxxxxxxxxxxxxxxxxxxxx`

## Using secrets.json (Optional Setup)

If you prefer to use `secrets.json` instead of user-secrets, update `Program.cs`:

```csharp
// Add this in Program.cs after builder.Configuration initialization
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
}
```

## Security Notes

       **Important Security Information:**

1. **Never commit `secrets.json`** - It's already in `.gitignore`
2. **User secrets are stored at:**
   - Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
   - macOS/Linux: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`
3. **For production:** Use Azure Key Vault, AWS Secrets Manager, or environment variables
4. **Rotate secrets regularly**
5. **Use different secrets for each environment**

## Current Active Secrets

The following secrets are currently configured via user-secrets:

```
    ConnectionStrings:DefaultConnection (SQL Server)
    JwtSettings:Secret (Authentication)
    SendGrid:ApiKey (Email - needs to be configured)
```

## Troubleshooting

### "User secrets not found"
```bash
cd src/WebAPI
dotnet user-secrets init
```

### "Can't connect to database"
- Check your SQL Server is running: `docker ps` or check SQL Server service
- Verify connection string: `dotnet user-secrets list`

### "Authentication not working"
- Verify JWT secret is set: `dotnet user-secrets list | grep JwtSettings`
- Secret must be at least 32 characters long

## Environment-Specific Configuration

### Local Development
- Use `secrets.json` or user-secrets (current)
- Database: localhost with Docker SQL Server

### Docker Development
- Use environment variables in `docker-compose.dev.yml`
- Secrets mounted via Docker secrets or environment variables

### Production
- Use Azure Key Vault or managed secrets service
- Never use `secrets.json` in production
- Set via environment variables or secrets manager

## Quick Reference

| Setting | Location | Priority |
|---------|----------|----------|
| `appsettings.json` | Project root | Lowest |
| `appsettings.Development.json` | Project root | Medium |
| User Secrets | Outside project | High |
| `secrets.json` | Project root | High (if configured) |
| Environment Variables | System | Highest |

## Next Steps

1. Copy `secrets.json.template` to `secrets.json`
2. Fill in your actual secret values
3. Run the application: `dotnet run --project src/WebAPI`
4. Verify secrets are loaded correctly

For Docker setup, see [DOCKER_SETUP.md](DOCKER_SETUP.md)

