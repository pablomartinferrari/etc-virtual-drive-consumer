# ETC Virtual Drive - SharePoint Storage Helper

A .NET Framework 4.6 library for seamless integration between desktop applications and SharePoint Online document libraries. Provides a simple file system-like API with enterprise-grade features including automatic retry logic, large file handling, caching, and mandatory audit logging.

## Features

- **Simple File System API** - Use familiar `File.ReadAllBytes`, `File.WriteAllBytes`, `Directory.CreateDirectory` patterns
- **Automatic Directory Creation** - Parent directories are created automatically
- **Large File Support** - Handles files over 100MB with chunked uploads and automatic retries
- **Sync & Async APIs** - Choose between synchronous operations (`ETCFile`) and non-blocking queued uploads (`ETCFileAsync`)
- **Intelligent Caching** - Automatic background uploads and local caching for files > 50MB
- **Resilience** - Built-in retry logic with exponential backoff for transient failures
- **URL Retrieval** - Get SharePoint URLs for files and folders
- **Mandatory Audit Logging** - All operations automatically logged to centralized SharePoint list
- **Multi-Site Support** - Connect to multiple SharePoint sites (Commercial, GCC High, etc.)

## Installation

### Option 1: NuGet Package from GitHub Packages (Recommended)

**First-time setup:** Add GitHub Packages as a NuGet source (requires GitHub account):

```powershell
# Replace YOUR_GITHUB_USERNAME and YOUR_GITHUB_PAT with your credentials
dotnet nuget add source "https://nuget.pkg.github.com/pablomartinferrari/index.json" `
    --name github `
    --username YOUR_GITHUB_USERNAME `
    --password YOUR_GITHUB_PAT `
    --store-password-in-clear-text
```

> **Note:** To get a GitHub Personal Access Token (PAT):
> 1. Go to https://github.com/settings/tokens
> 2. Generate a new token (classic) with `read:packages` permission
> 3. Use that token as YOUR_GITHUB_PAT above

**Install the package:**

```powershell
Install-Package ETCStorageHelper -Version 1.0.0 -Source github
```

Or add to your `.csproj`:

```xml
<PackageReference Include="ETCStorageHelper" Version="1.0.0" />
```

### Option 2: Build from Source

```bash
git clone https://github.com/pablomartinferrari/etc-virtual-drive.git
cd etc-virtual-drive
cd distribute
.\1-Build-Release.ps1
.\2-Create-NuGetPackage.ps1
```

## Quick Start

> **Note:** Azure AD app registration and permission setup are handled as part of the `ETCStorageHelper` library documentation.  
> As an application developer, you typically only need to provide the correct configuration values (TenantId, ClientId, ClientSecret, SiteUrl, LibraryName).

### 1. Configuration

Add to your `App.config`:

```xml
<appSettings>
  <!-- SharePoint Commercial Site -->
  <add key="ETCStorage.Commercial.TenantId" value="your-tenant-id" />
  <add key="ETCStorage.Commercial.ClientId" value="your-client-id" />
  <add key="ETCStorage.Commercial.ClientSecret" value="your-client-secret" />
  <add key="ETCStorage.Commercial.SiteUrl" value="https://yourtenant.sharepoint.com/sites/your-site" />
  <add key="ETCStorage.Commercial.LibraryName" value="Documents" />
</appSettings>
```

### 2. Basic Usage

```csharp
using ETCStorageHelper;

// Initialize connection (logging is mandatory)
var site = SharePointSite.FromConfig(
    name: "Commercial",
    configPrefix: "ETCStorage.Commercial",
    userId: "jdoe",
    userName: "John Doe",
    applicationName: "MyApp"
);

// Create directory
ETCDirectory.CreateDirectory("ClientA/2025/Job001/Reports", site);

// Write file
byte[] pdfData = File.ReadAllBytes("report.pdf");
ETCFile.WriteAllBytes("ClientA/2025/Job001/Reports/report.pdf", pdfData, site);

// Read file
byte[] data = ETCFile.ReadAllBytes("ClientA/2025/Job001/Reports/report.pdf", site);

// Get SharePoint URL
string url = ETCFile.GetFileUrl("ClientA/2025/Job001/Reports/report.pdf", site);
Console.WriteLine($"View at: {url}");
```

> **Note:** You do **not** need to call `ETCDirectory.CreateDirectory` before writing.  
> `ETCFile.WriteAllBytes` (and other write methods) will automatically create any missing parent folders.

## Advanced Features

### Large File Handling

Files over 50MB are automatically handled with:

- **Chunked uploads** - 4MB chunks with retry logic
- **Background processing options**
  - **Synchronous path**: `ETCFile.WriteAllBytes` blocks until upload (with retries) completes.
  - **Async/queued path**: `ETCFileAsync.WriteAllBytesAsync` queues the upload and returns immediately with an `UploadHandle`.
- **Local caching** - Fast subsequent reads

```csharp
// Option 1: Synchronous (simpler, but blocks until done)
byte[] largeFile = File.ReadAllBytes("large-report.pdf"); // 200MB
ETCFile.WriteAllBytes("Reports/large-report.pdf", largeFile, site);

// Option 2: Async/queued (non-blocking)
var handle = ETCFileAsync.WriteAllBytesAsync(
    "Reports/large-report.pdf",
    largeFile,
    site,
    onSuccess: path => Console.WriteLine($"Upload complete: {path}"),
    onError: (path, ex) => Console.WriteLine($"Upload failed for {path}: {ex.Message}")
);
// Returns immediately; upload continues in background

// First read downloads and caches
byte[] data1 = ETCFile.ReadAllBytes("Reports/large-report.pdf", site); // ~80 seconds

// Subsequent reads use cache
byte[] data2 = ETCFile.ReadAllBytes("Reports/large-report.pdf", site); // ~200ms
```

### Automatic Retry & Resilience

All SharePoint operations include:

- **Exponential backoff** - Automatic retry with increasing delays
- **Jitter** - Random delays to prevent thundering herd
- **Transient error handling** - Retries on network issues, throttling, etc.

```csharp
// This will automatically retry on:
// - Network timeouts
// - SharePoint throttling (429 errors)
// - Temporary service unavailability
// - Authentication token expiration
ETCFile.WriteAllBytes("path/file.pdf", data, site);
```

### Centralized Audit Logging

**All operations are automatically logged** to a SharePoint list named "ETC Storage Logs". Logging cannot be disabled.

Log entries include:

- **User** - UserId and UserName (mandatory at initialization)
- **Operation** - CreateDirectory, WriteFile, ReadFile, CopyFile, DeleteFile
- **Level** - Info (< 10s), Warning (10-30s), Error (failed or > 30s)
- **Path** - File/folder path
- **Duration** - Operation time in milliseconds
- **File Size** - In megabytes
- **Success** - True/false
- **Machine Name** - Computer that performed the operation
- **Application** - Application name
- **Timestamp** - When operation occurred

**View logs at:** `https://yourtenant.sharepoint.com/sites/your-site/Lists/ETC%20Storage%20Logs`

### Multi-Site Support

Connect to multiple SharePoint sites (e.g., Commercial vs. GCC High):

```csharp
// Commercial site (config-driven)
var commercial = SharePointSite.FromConfig(
    name: "Commercial",
    configPrefix: "ETCStorage.Commercial",
    userId: "jdoe",
    userName: "John Doe",
    applicationName: "MyApp"
);

// GCC High site (for CUI data, also config-driven)
var gccHigh = SharePointSite.FromConfig(
    name: "GCC High",
    configPrefix: "ETCStorage.GCCHigh",
    userId: "jdoe",
    userName: "John Doe",
    applicationName: "MyApp"
);

// Route data based on classification
if (isCUI)
{
    ETCFile.WriteAllBytes("Classified/report.pdf", data, gccHigh);
}
else
{
    ETCFile.WriteAllBytes("Standard/report.pdf", data, commercial);
}
```

## API Reference

### ETCFile Class

| Method                            | Description                                          |
| --------------------------------- | ---------------------------------------------------- |
| `WriteAllBytes(path, data, site)` | Write binary data to file (auto-creates directories) |
| `WriteAllText(path, text, site)`  | Write text to file                                   |
| `ReadAllBytes(path, site)`        | Read binary data from file                           |
| `ReadAllText(path, site)`         | Read text from file                                  |
| `Exists(path, site)`              | Check if file exists                                 |
| `Delete(path, site)`              | Delete file                                          |
| `Copy(source, dest, site)`        | Copy file                                            |
| `GetFileUrl(path, site)`          | Get SharePoint URL for file                          |

### ETCDirectory Class

| Method                          | Description                              |
| ------------------------------- | ---------------------------------------- |
| `CreateDirectory(path, site)`   | Create directory (and parents if needed) |
| `Exists(path, site)`            | Check if directory exists                |
| `Delete(path, site, recursive)` | Delete directory                         |
| `GetFiles(path, site)`          | List files in directory                  |
| `GetDirectories(path, site)`    | List subdirectories                      |
| `GetFolderUrl(path, site)`      | Get SharePoint URL for folder            |

### ETCPath Class

| Method                              | Description                                           |
| ----------------------------------- | ----------------------------------------------------- |
| `Combine(path1, path2, ...)`        | Combine path segments (handles SharePoint separators) |
| `GetDirectoryName(path)`            | Get parent directory                                  |
| `GetFileName(path)`                 | Get file name                                         |
| `GetExtension(path)`                | Get file extension                                    |
| `GetFileNameWithoutExtension(path)` | Get file name without extension                       |

## Configuration Options

### SharePointSite Properties

| Property               | Required | Description                                          |
| ---------------------- | -------- | ---------------------------------------------------- |
| `Name`                 | Yes      | Friendly name for the site                           |
| `TenantId`             | Yes      | Azure AD tenant ID                                   |
| `ClientId`             | Yes      | Azure AD app registration client ID                  |
| `ClientSecret`         | Yes      | Azure AD app registration client secret              |
| `SiteUrl`              | Yes      | Full SharePoint site URL                             |
| `LibraryName`          | Yes      | Document library name (case-sensitive)               |
| `UserId`               | Yes      | User ID for audit logging                            |
| `UserName`             | Yes      | User display name for audit logging                  |
| `ApplicationName`      | Optional | Application name for audit logging                   |
| `AutoAsyncThresholdMB` | Optional | File size threshold for auto-caching (default: 50MB) |

## Performance Benchmarks

> **Note:** Small writes run synchronously end‑to‑end; large writes use background upload, so “Instant” below means _the method returns quickly while the upload continues in the background_.

| Operation | File Size | Time (First)                     | Time (Cached) |
| --------- | --------- | -------------------------------- | ------------- |
| Write     | 1 MB      | ~500ms (synchronous end-to-end)  | N/A           |
| Write     | 100 MB    | “Instant”\* (queued, background) | N/A           |
| Write     | 200 MB    | “Instant”\* (queued, background) | N/A           |
| Read      | 1 MB      | ~300ms                           | ~50ms         |
| Read      | 100 MB    | ~80s                             | ~200ms        |
| Read      | 200 MB    | ~160s                            | ~400ms        |

\*Upload continues after method returns (background upload queue)

## Troubleshooting

### "Library 'XXX' not found in site"

**Cause:** Library name is case-sensitive or doesn't exist.

**Fix:** Check exact library name:

```powershell
$siteUrl = "https://yourtenant.sharepoint.com/sites/your-site"
Connect-PnPOnline -Url $siteUrl -Interactive
Get-PnPList | Where-Object {$_.BaseTemplate -eq 101} | Select-Object Title
```

### Files not appearing in SharePoint

**Cause:** Large files still uploading in background.

**Fix:** Check logs in SharePoint "ETC Storage Logs" list or wait 30-60 seconds for upload to complete.

### Authentication errors

**Cause:** Missing API permissions or admin consent.

**Fix:**

1. Go to Azure Portal > App Registrations > Your App
2. Click "API Permissions"
3. Ensure `Sites.ReadWrite.All` (Application) is added
4. Click "Grant admin consent"
5. Wait 5 minutes for permissions to propagate

## Architecture

The library consists of:

- **`ETCFile`** - File operations (read, write, delete, copy)
- **`ETCDirectory`** - Directory operations (create, delete, list)
- **`ETCPath`** - Path manipulation utilities
- **`SharePointClient`** - Core Microsoft Graph API integration
- **`AuthenticationManager`** - Azure AD authentication
- **`RetryPolicy`** - Exponential backoff retry logic
- **`LocalFileCache`** - Caching system for large files
- **`BackgroundUploadQueue`** - Async upload manager
- **`SharePointListLogger`** - Centralized audit logging to SharePoint

## License

Internal use only - Environmental Testing & Consulting (ETC)  
Copyright © 2025 Environmental Testing & Consulting

This software is proprietary and confidential. Permission is granted only to
employees and contractors of Environmental Testing & Consulting (ETC) to use,
copy, and modify this software within ETC-managed environments for internal
business purposes. Any other use, including redistribution outside ETC,
sublicensing, or use for the benefit of third parties, is strictly prohibited
without prior written consent of ETC.

THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT.

## Support

For issues, feature requests, or questions:

- **GitHub Issues**: https://github.com/your-org/etc-virtual-drive/issues
- **Documentation**: See `src/ETCStorageHelper/README.md` for detailed API documentation
