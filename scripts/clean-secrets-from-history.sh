#!/bin/bash
# Script to remove secrets from git history
# WARNING: This rewrites git history. Only use on new repos or with team approval.

echo "⚠️  This script will rewrite git history to remove secrets."
echo "Only proceed if this is a new repository or you have team approval."
read -p "Continue? (yes/no): " confirm

if [ "$confirm" != "yes" ]; then
    echo "Aborted."
    exit 1
fi

# Backup current branch
echo "Creating backup branch..."
git branch backup-before-secret-cleanup

# Remove secrets from all commits
echo "Removing secrets from git history..."

# Remove appsettings.Development.json from all commits
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch src/WebAPI/appsettings.Development.json" \
  --prune-empty --tag-name-filter cat -- --all

# Remove launchSettings.json from all commits
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch src/WebAPI/Properties/launchSettings.json" \
  --prune-empty --tag-name-filter cat -- --all

# Clean up refs
git for-each-ref --format="delete %(refname)" refs/original | git update-ref --stdin
git reflog expire --expire=now --all
git gc --prune=now --aggressive

echo "✅ History cleaned!"
echo ""
echo "Next steps:"
echo "1. Review your commits: git log --oneline"
echo "2. If satisfied, force push: git push origin main --force"
echo "3. If something went wrong, restore: git reset --hard backup-before-secret-cleanup"

