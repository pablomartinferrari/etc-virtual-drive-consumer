# ETC Storage Helper - Test Application

A comprehensive test application for the ETCStorageHelper library that allows you to test all API endpoints and features.

## Features

This test application provides complete coverage of:

### ETCFile API
- `WriteAllBytes` - Write binary data
- `WriteAllText` - Write text data
- `ReadAllBytes` - Read binary data
- `ReadAllText` - Read text data
- `Exists` - Check file existence
- `Delete` - Delete files
- `Copy` - Copy files
- `GetFileUrl` - Get SharePoint URLs

### ETCDirectory API
- `CreateDirectory` - Create directories (with auto-parent creation)
- `Exists` - Check directory existence
- `Delete` - Delete directories (recursive and non-recursive)
- `GetFiles` - List files in directory
- `GetDirectories` - List subdirectories
- `GetFolderUrl` - Get SharePoint folder URLs

### ETCPath API
- `Combine` - Combine path segments
- `GetDirectoryName` - Get parent directory
- `GetFileName` - Get file name
- `GetExtension` - Get file extension
- `GetFileNameWithoutExtension` - Get name without extension

### ETCFileAsync API
- `WriteAllBytesAsync` - Async/queued uploads for large files
- Cache testing for large file reads
- Performance benchmarking

### Integration Tests
- Complete end-to-end workflow simulation
- Real-world scenario testing
- Multi-step operations

## Setup

### 1. Configure App.config

Edit `App.config` and provide your SharePoint credentials:

```xml
<appSettings>
  <!-- SharePoint Configuration -->
  <add key="ETCStorage.Commercial.TenantId" value="your-tenant-id-here" />
  <add key="ETCStorage.Commercial.ClientId" value="your-client-id-here" />
  <add key="ETCStorage.Commercial.ClientSecret" value="your-client-secret-here" />
  <add key="ETCStorage.Commercial.SiteUrl" value="https://yourtenant.sharepoint.com/sites/your-site" />
  <add key="ETCStorage.Commercial.LibraryName" value="Documents" />
  
  <!-- Test User Configuration -->
  <add key="Test.UserId" value="your-user-id" />
  <add key="Test.UserName" value="Your Name" />
  <add key="Test.ApplicationName" value="ETCStorageHelper.TestApp" />
  
  <!-- Test Base Path -->
  <add key="Test.BasePath" value="ETCStorageTest" />
</appSettings>
```

### 2. Build and Run

```powershell
# Build the project
dotnet build

# Run the test application
dotnet run
```

Or open in Visual Studio and press F5.

## Usage

The application presents an interactive menu:

```
==============================================
                 MAIN MENU
==============================================
1. Test ETCFile Methods
2. Test ETCDirectory Methods
3. Test ETCPath Methods
4. Test ETCFileAsync Methods (Large Files)
5. Run Complete Integration Test
6. Cleanup Test Data
0. Exit
==============================================
```

### Menu Options

1. **Test ETCFile Methods** - Tests all file operations including read, write, delete, copy, and URL retrieval
2. **Test ETCDirectory Methods** - Tests directory creation, listing, deletion (recursive/non-recursive)
3. **Test ETCPath Methods** - Tests path manipulation utilities (runs locally, no SharePoint connection needed)
4. **Test ETCFileAsync Methods** - Tests large file handling (60MB+) with async uploads and caching
5. **Run Complete Integration Test** - Simulates a real-world project workflow with multiple operations
6. **Cleanup Test Data** - Removes all test data from SharePoint (requires confirmation)

## Test Data

All test data is created under the base path specified in `App.config` (default: `ETCStorageTest`).

Test files include:
- Binary files (1KB - 60MB)
- Text files
- PDF-like files
- CSV files
- Multiple file versions

## Performance Testing

The application measures and displays:
- Operation duration (milliseconds/seconds)
- File sizes
- Upload/download speeds
- Cache performance

### Expected Performance (from README)

| Operation | File Size | Time (First)                    | Time (Cached) |
|-----------|-----------|----------------------------------|---------------|
| Write     | 1 MB      | ~500ms (synchronous)             | N/A           |
| Write     | 60 MB     | "Instant" (queued, background)   | N/A           |
| Read      | 1 MB      | ~300ms                           | ~50ms         |
| Read      | 60 MB     | ~80s                             | ~200ms        |

## Audit Logging

All operations are automatically logged to the SharePoint list "ETC Storage Logs". You can view logs at:

```
https://yourtenant.sharepoint.com/sites/your-site/Lists/ETC%20Storage%20Logs
```

## Troubleshooting

### Authentication Errors

If you get authentication errors:
1. Verify your Azure AD app has `Sites.ReadWrite.All` permission
2. Ensure admin consent has been granted
3. Wait 5 minutes for permissions to propagate
4. Check that TenantId, ClientId, and ClientSecret are correct

### Library Not Found

If you get "Library 'XXX' not found" errors:
- Library names are case-sensitive
- Verify the exact library name in SharePoint
- Common names: "Documents", "Shared Documents"

### Connection Issues

If SharePoint connection fails:
- Verify SiteUrl is correct
- Check network connectivity
- Ensure firewall allows HTTPS to SharePoint

## Project Structure

```
ETCStorageHelper.TestApp/
├── Program.cs                  # Main menu and initialization
├── ETCFileTests.cs             # ETCFile API tests
├── ETCDirectoryTests.cs        # ETCDirectory API tests
├── ETCPathTests.cs             # ETCPath API tests
├── ETCFileAsyncTests.cs        # Large file and async tests
├── IntegrationTests.cs         # End-to-end integration tests
├── TestDataGenerator.cs        # Test data generation utilities
├── App.config                  # Configuration file
└── ETCStorageHelper.TestApp.csproj
```

## Notes

- Large file tests (60MB) may take several minutes
- All tests use the base path from config to avoid interfering with production data
- Use "Cleanup Test Data" option to remove all test files when done
- Tests are designed to be idempotent (can run multiple times)

## Support

For issues with the ETCStorageHelper library itself, see:
- https://github.com/pablomartinferrari/etc-virtual-drive

For issues with this test application, check the configuration and ensure SharePoint connectivity.


