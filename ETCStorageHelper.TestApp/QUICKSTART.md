# Quick Start Guide

Get up and running with the ETCStorageHelper test application in 5 minutes!

## Prerequisites

- .NET Framework 4.6 or higher
- Visual Studio 2019+ or .NET CLI
- SharePoint Online site with Document Library
- Azure AD app registration with Sites.ReadWrite.All permission

## Step-by-Step Setup

### 1. Configure Your Credentials

Open `App.config` and replace these values:

```xml
<!-- Your Azure AD Tenant ID (GUID) -->
<add key="ETCStorage.Commercial.TenantId" value="12345678-1234-1234-1234-123456789abc" />

<!-- Your Azure AD App Registration Client ID (GUID) -->
<add key="ETCStorage.Commercial.ClientId" value="87654321-4321-4321-4321-cba987654321" />

<!-- Your Azure AD App Registration Client Secret -->
<add key="ETCStorage.Commercial.ClientSecret" value="your-secret-here~abc123" />

<!-- Your SharePoint Site URL -->
<add key="ETCStorage.Commercial.SiteUrl" value="https://yourtenant.sharepoint.com/sites/your-site" />

<!-- Your Document Library Name (case-sensitive!) -->
<add key="ETCStorage.Commercial.LibraryName" value="Documents" />

<!-- Your User Info -->
<add key="Test.UserId" value="jdoe" />
<add key="Test.UserName" value="John Doe" />
```

### 2. Build the Project

**Using Visual Studio:**
1. Open `ETCStorageHelper.TestApp.sln`
2. Press `Ctrl+Shift+B` to build

**Using Command Line:**
```powershell
cd ETCStorageHelper.TestApp
dotnet build
```

### 3. Run the Application

**Using Visual Studio:**
1. Press `F5` or click "Start"

**Using Command Line:**
```powershell
dotnet run
```

## First Test Run

Once the application starts, try this sequence:

1. **Test ETCPath Methods** (Option 3)
   - Runs locally, no SharePoint connection needed
   - Verifies the application is working

2. **Test ETCFile Methods** (Option 1)
   - Tests basic file operations
   - Creates a few test files in SharePoint
   - Takes ~5-10 seconds

3. **Run Complete Integration Test** (Option 5)
   - Tests everything together
   - Simulates real-world usage
   - Takes ~30-60 seconds

4. **Cleanup Test Data** (Option 6)
   - Removes all test files
   - Type "yes" to confirm

## What Gets Created

The test application creates this structure in SharePoint:

```
ETCStorageTest/
├── FileTests/
│   ├── test-binary.dat
│   ├── test-text.txt
│   └── test-text-copy.txt
├── DirectoryTests/
│   ├── Level1/Level2/Level3/
│   └── ListingTest/
├── AsyncTests/
│   ├── small-1mb.dat
│   ├── medium-10mb.dat
│   └── large-60mb.dat
└── IntegrationTest/
    └── ClientA/2025/Project001/
```

## Viewing Test Results

### In the Console
All test results appear in the console with:
- ✓ Green checkmarks for success
- ✗ Red X for failures
- Timing information
- File sizes and paths

### In SharePoint
1. Open your SharePoint site
2. Navigate to the Document Library
3. Look for the `ETCStorageTest` folder

### Audit Logs
View all operations at:
```
https://yourtenant.sharepoint.com/sites/your-site/Lists/ETC%20Storage%20Logs
```

## Common Issues

### "Failed to initialize SharePoint connection"
- Check your TenantId, ClientId, and ClientSecret
- Verify Azure AD app has Sites.ReadWrite.All permission
- Ensure admin consent has been granted

### "Library 'XXX' not found"
- Library name is case-sensitive
- Use exact name from SharePoint (usually "Documents")
- Check with: `Get-PnPList | Where-Object {$_.BaseTemplate -eq 101} | Select-Object Title`

### "Access denied"
- Verify Azure AD app permissions
- Grant admin consent in Azure Portal
- Wait 5 minutes for permissions to propagate

## Testing Large Files

Option 4 (Test ETCFileAsync Methods) includes large file tests:
- **1MB file**: Tests synchronous upload (~500ms)
- **10MB file**: Tests medium file handling (~2-3s)
- **60MB file**: Tests async/queued upload with background processing

⚠️ Large file tests may take 5-10 minutes to complete.

## Performance Expectations

You should see timing similar to:

| Operation           | Expected Time |
|---------------------|---------------|
| Write 1MB file      | ~500ms        |
| Read 1MB file       | ~300ms        |
| Write 10MB file     | ~2-3s         |
| Read 10MB (cached)  | ~200ms        |
| Create directory    | ~500ms        |
| List files          | ~300ms        |

## Next Steps

After verifying everything works:

1. **Explore the Code**
   - Look at `ETCFileTests.cs` for file operation examples
   - Check `IntegrationTests.cs` for real-world patterns
   - Review `TestDataGenerator.cs` for test data creation

2. **Modify Tests**
   - Add your own test scenarios
   - Test with your specific file types
   - Adjust file sizes for your needs

3. **Use in Your Application**
   - Copy patterns from the test app
   - Reference the main README for detailed API docs
   - Follow the integration test for best practices

## Cleanup

When finished testing:

1. Run option 6 (Cleanup Test Data)
2. Type "yes" to confirm
3. All test files are removed from SharePoint

## Need Help?

- Review the main [README.md](README.md) for detailed documentation
- Check the main library README at the repository root
- Verify Azure AD app configuration
- Check SharePoint "ETC Storage Logs" list for operation details

## Tips

- Start with small file tests before large files
- Use option 3 (ETCPath) first - it runs without SharePoint
- Keep the test base path (`ETCStorageTest`) separate from production data
- Large file tests can be skipped if not needed
- All tests are idempotent - safe to run multiple times


