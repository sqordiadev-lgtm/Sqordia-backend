#!/bin/bash
# Script to remove secrets from git history by rewriting file contents

set -e

echo "⚠️  This will rewrite git history to remove secrets."
echo "Backing up current state..."

# Backup
git branch backup-before-secret-fix || true

echo "Removing secrets from all commits..."

# Use git filter-branch with proper escaping
git filter-branch --force --tree-filter '
# Fix appsettings.json - replace Google OAuth Client Secret
if [ -f src/WebAPI/appsettings.json ]; then
  sed -i.bak "s|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|\${GOOGLE_OAUTH_CLIENT_SECRET}|g" src/WebAPI/appsettings.json
  rm -f src/WebAPI/appsettings.json.bak 2>/dev/null || true
fi

# Fix docker-compose.dev.yml - remove SendGrid API key default
if [ -f docker-compose.dev.yml ]; then
  sed -i.bak "s|SENDGRID_API_KEY:-SG\.u-GNMW9RR-ydoMlQb4U8-w\.ddSO4zJlzVGujLVRPS5jCWcXv1KO9LU_hOhyW82sBZ8|SENDGRID_API_KEY:-|g" docker-compose.dev.yml
  rm -f docker-compose.dev.yml.bak 2>/dev/null || true
fi
' --prune-empty --tag-name-filter cat -- --all

# Clean up
echo "Cleaning up refs..."
git for-each-ref --format="delete %(refname)" refs/original | git update-ref --stdin || true
git reflog expire --expire=now --all
git gc --prune=now --aggressive

echo "✅ History cleaned!"
echo ""
echo "Verification - checking if secrets still exist in history:"
echo "Checking appsettings.json in initial commit:"
git show 6b068cc:src/WebAPI/appsettings.json 2>/dev/null | grep -i "GOCSPX" || echo "✅ No Google OAuth secret found"
echo ""
echo "Next step: Review commits, then force push with:"
echo "  git push origin main --force"

