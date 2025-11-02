# Push Instructions

Your stored credentials have been cleared. To push to GitHub, run this command in your terminal:

```bash
cd /Users/fsawadogo/Desktop/Sqordia-backend
git push origin main --force
```

## When prompted for credentials:

1. **Username**: Enter your GitHub username (or the account that has access to `sqordiadev-lgtm/Sqordia-backend`)

2. **Password**: Enter a **Personal Access Token** (NOT your GitHub password)
   - If you don't have one, create it at: https://github.com/settings/tokens
   - Generate new token (classic)
   - Select scope: `repo` (full control)
   - Copy the token and use it as the password

## Alternative: Use token in URL directly

If you prefer, you can push using the token directly:

```bash
git push https://YOUR_TOKEN@github.com/sqordiadev-lgtm/Sqordia-backend.git main --force
```

Replace `YOUR_TOKEN` with your actual Personal Access Token.

---

**Note**: The repository has been cleaned - all commit history removed and only 1 fresh commit with no secrets remains. This force push will replace all history on GitHub.

