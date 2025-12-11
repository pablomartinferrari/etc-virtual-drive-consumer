# Setup Instructions

## Configuration

1. Copy `App.config.template` to `App.config`:
   ```powershell
   Copy-Item ETCStorageHelper.TestApp\App.config.template ETCStorageHelper.TestApp\App.config
   ```

2. Edit `App.config` with your actual credentials:
   - Replace `YOUR-TENANT-ID-HERE` with your Azure AD Tenant ID
   - Replace `YOUR-CLIENT-ID-HERE` with your Azure AD App Registration Client ID
   - Replace `YOUR-CLIENT-SECRET-HERE` with your Azure AD App Registration Client Secret
   - Replace the SharePoint site URL with your actual site
   - Update user information

3. **Never commit `App.config` to Git** - it's already in `.gitignore`

## Important Security Note

?? **The `App.config` file contains sensitive credentials and should NEVER be committed to source control.**

The repository includes:
- ? `App.config.template` - Template with placeholder values (safe to commit)
- ? `App.config` - Your actual config with secrets (excluded from Git via `.gitignore`)
