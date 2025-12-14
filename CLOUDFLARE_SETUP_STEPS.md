# Cloudflare Setup - Step by Step

Follow these steps to set up Cloudflare for your Sqordia backend.

## Prerequisites

- ✅ Static IP: `34.19.227.174` (reserved)
- ✅ Domain name (e.g., sqordia.com)
- ⏳ Cloudflare account (we'll create this)

## Step 1: Sign Up for Cloudflare

1. **Go to**: https://dash.cloudflare.com/sign-up
2. **Enter your email** and create a password
3. **Verify your email** (check inbox)
4. **Select Free plan** when prompted

**Time**: 2-5 minutes

---

## Step 2: Add Your Domain to Cloudflare

1. **In Cloudflare dashboard**, click **"Add a Site"** (big button)
2. **Enter your domain** (e.g., `sqordia.com`)
   - Don't include `www` or `http://`
   - Just the domain: `sqordia.com`
3. **Click "Add site"**
4. **Select Free plan** (scroll down if needed)
5. **Click "Continue"**

**Time**: 1-2 minutes

---

## Step 3: Update Nameservers

Cloudflare will show you **2 nameservers**, for example:
- `alice.ns.cloudflare.com`
- `bob.ns.cloudflare.com`

### At Your Domain Registrar:

1. **Log in** to your domain registrar (where you bought the domain)
2. **Find DNS/Nameserver settings**:
   - Google Domains: Settings → DNS → Custom name servers
   - Namecheap: Domain List → Manage → Nameservers
   - GoDaddy: DNS → Nameservers → Change
   - Cloudflare Registrar: Already done if domain is with Cloudflare
3. **Replace existing nameservers** with Cloudflare's 2 nameservers
4. **Save changes**

### Back in Cloudflare:

1. **Click "Continue"** after updating nameservers
2. **Wait for verification** (5-30 minutes)
   - Cloudflare will check automatically
   - You'll get an email when it's verified

**Time**: 5-30 minutes (mostly waiting)

---

## Step 4: Add DNS A Record

Once your domain is verified in Cloudflare:

1. **Go to**: DNS → Records (in Cloudflare dashboard)
2. **Click**: "Add record" button
3. **Configure the record**:
   - **Type**: Select `A`
   - **Name**: Enter `api` (this creates `api.yourdomain.com`)
     - Or use `backend` for `backend.yourdomain.com`
     - Or use `@` for root domain `yourdomain.com`
   - **IPv4 address**: Enter `34.19.227.174`
   - **Proxy status**: **Click the cloud icon** to turn it **orange** (Proxied) ⚠️ **CRITICAL!**
     - Gray cloud = DNS only (no SSL)
     - Orange cloud = Proxied (free SSL + CDN)
   - **TTL**: Leave as "Auto"
4. **Click "Save"**

**Important**: Make sure the cloud is **orange** (Proxied), not gray!

**Time**: 1 minute

---

## Step 5: Configure SSL/TLS

1. **Go to**: SSL/TLS → Overview (in Cloudflare dashboard)
2. **Select**: **Full** (recommended)
   - **Full**: Encrypts connection between visitor ↔ Cloudflare ↔ your server
   - **Full (strict)**: Same as Full, but validates your server's certificate (use if you have a cert)
3. **SSL certificate will be automatically provisioned** (usually within minutes)

**Time**: 1 minute (certificate provisioning: 5-60 minutes)

---

## Step 6: Wait for DNS Propagation

- **DNS changes** usually propagate within **5-60 minutes**
- **SSL certificate** provisioning can take **5-60 minutes**

### Check Status:

```bash
# Test DNS resolution
nslookup api.yourdomain.com

# Should return Cloudflare's proxy IP (not your static IP directly)
```

---

## Step 7: Test Your Setup

### Test DNS:

```bash
nslookup api.yourdomain.com
```

### Test HTTP:

```bash
curl http://api.yourdomain.com:8080/health
```

### Test HTTPS (if SSL is ready):

```bash
curl https://api.yourdomain.com:8080/health
```

### Test Login:

```bash
curl -X POST http://api.yourdomain.com:8080/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@sqordia.com", "password": "Sqordia2025!"}'
```

---

## Optional: Remove Port from URL

If you want to access your API without `:8080`:

### Option A: Page Rules (Free Plan - 3 rules)

1. **Go to**: Rules → Page Rules
2. **Click**: "Create Page Rule"
3. **Configure**:
   - **URL**: `http://api.yourdomain.com:8080/*`
   - **Settings**:
     - **Forwarding URL**: `301 Permanent Redirect`
     - **Destination URL**: `https://api.yourdomain.com/*`
4. **Click**: "Save and Deploy"

### Option B: Cloudflare Workers (More Flexible)

Create a worker to rewrite URLs and remove the port.

---

## Update Application Configuration

After your domain is working, update:

### 1. secrets.json

```json
{
  "Deployment": {
    "URL": "https://api.yourdomain.com",
    "HealthEndpoint": "https://api.yourdomain.com/health"
  },
  "GoogleOAuth": {
    "RedirectUri": "https://api.yourdomain.com/api/v1/auth/google/callback"
  }
}
```

### 2. Google OAuth Redirect URI

1. Go to [Google Cloud Console](https://console.cloud.google.com)
2. APIs & Services → Credentials
3. Edit your OAuth 2.0 Client
4. Add: `https://api.yourdomain.com/api/v1/auth/google/callback`
5. Save

---

## Troubleshooting

### DNS Not Resolving?

- Wait longer (can take up to 48 hours, usually 5-60 minutes)
- Check nameservers are correct at registrar
- Verify DNS record in Cloudflare dashboard

### SSL Not Working?

- Check proxy status is **orange** (Proxied)
- Check SSL/TLS mode is **Full**
- Wait for certificate provisioning (up to 24 hours, usually minutes)

### Can't Connect?

- Check GCP firewall allows port 8080
- Verify instance is running
- Test direct IP: `curl http://34.19.227.174:8080/health`

---

## Quick Reference

- **Static IP**: `34.19.227.174`
- **DNS Record**: `api` → `34.19.227.174` (Proxied)
- **SSL Mode**: Full
- **API URL**: `https://api.yourdomain.com:8080`

---

## Need Help?

- **Cloudflare Support**: https://support.cloudflare.com
- **Documentation**: https://developers.cloudflare.com/dns/
- **Community**: https://community.cloudflare.com

