# ETCStorageHelper v1.0.2 - Release Summary

## 🎉 What's New

### ✅ Added Missing Delete Methods

**Problem:** `ETCDirectory` class was missing a `Delete()` method, while `ETCFile` had one.

**Solution:** 
- ✅ Added `ETCDirectory.Delete(path, site, recursive)` - Delete folders (with recursive option)
- ✅ Added `ETCFileAsync.DeleteAsync(path, site, ...)` - Non-blocking background deletion
- ✅ Added `SharePointClient.DeleteFolderAsync(path)` - Underlying Graph API implementation

### 📚 Clarified Synchronous vs Async Behavior

**Problem:** It wasn't clear whether methods were blocking or non-blocking.

**Solution:**
- ✅ Updated README with clear "Blocks?" column in API reference tables
- ✅ Added dedicated section explaining synchronous vs asynchronous operations
- ✅ Documented that `ETCFile` and `ETCDirectory` methods are **synchronous** (blocking)
- ✅ Documented that `ETCFileAsync` methods are **asynchronous** (non-blocking)

## 📋 Complete API Changes

### New Methods Added

```csharp
// ETCDirectory - Now has Delete!
ETCDirectory.Delete(string path, SharePointSite site, bool recursive = false)

// ETCFileAsync - Now has DeleteAsync!
UploadHandle ETCFileAsync.DeleteAsync(
    string path, 
    SharePointSite site,
    Action<string> onSuccess = null,
    Action<string, Exception> onError = null
)
```

### Usage Examples

#### Delete a Directory (Synchronous)
```csharp
// Delete empty directory
ETCDirectory.Delete("ClientA/Archive/2023", site);

// Delete directory and all contents (recursive)
ETCDirectory.Delete("ClientA/Archive/2023", site, recursive: true);
```

#### Delete a File (Asynchronous)
```csharp
// Queue deletion - returns immediately
var handle = ETCFileAsync.DeleteAsync(
    "ClientA/OldReport.pdf",
    site,
    onSuccess: path => Console.WriteLine($"Deleted: {path}"),
    onError: (path, ex) => Console.WriteLine($"Failed: {ex.Message}")
);

// Check status
Console.WriteLine($"Deletion queued: {handle.UploadId}");
```

## 🔄 Understanding Sync vs Async

### Synchronous Methods (Blocking)

These methods **block** your application until the operation completes:

```csharp
// ⏸️ BLOCKS - May take seconds/minutes for large files
ETCFile.WriteAllBytes("report.pdf", data, site);        // Waits until upload finishes
byte[] data = ETCFile.ReadAllBytes("report.pdf", site); // Waits until download finishes
ETCDirectory.Delete("folder", site);                    // Waits until deletion completes
```

**When to use:**
- Simple operations where blocking is acceptable
- Small files (< 10 MB)
- When you need immediate confirmation of success/failure

### Asynchronous Methods (Non-Blocking)

These methods return **immediately** and continue in the background:

```csharp
// ⚡ NON-BLOCKING - Returns immediately
var handle = ETCFileAsync.WriteAllBytesAsync(
    "report.pdf", 
    data, 
    site,
    onSuccess: path => Console.WriteLine("Done!"),
    onError: (path, ex) => Console.WriteLine($"Failed: {ex}")
);
// Continue working while upload happens in background
DoOtherWork();

// Optional: Wait for completion later
ETCFileAsync.WaitForUploads(site, timeoutSeconds: 300);
```

**When to use:**
- Large files (> 50 MB)
- UI applications (prevent freezing)
- Batch operations (upload multiple files)
- When you want to continue working while operation completes

### Cached Reads (Hybrid)

`ReadAllBytesCached` is synchronous but **much faster** after first read:

```csharp
// First read: ~80 seconds (downloads and caches)
byte[] data1 = ETCFileAsync.ReadAllBytesCached("large-file.pdf", site);

// Subsequent reads: ~200ms (from cache)
byte[] data2 = ETCFileAsync.ReadAllBytesCached("large-file.pdf", site);
```

## 📦 Package Contents

**Version 1.0.2** includes:
- ✅ `ETCStorageHelper.dll` (87 KB) - +3 KB with new methods
- ✅ `ETCStorageHelper.pdb` (217.5 KB) - Debug symbols
- ✅ `ETCStorageHelper.xml` (35.93 KB) - IntelliSense documentation
- ✅ Target Framework: .NET Framework 4.6

## 🔄 Upgrading from v1.0.1

### Option 1: Visual Studio NuGet Manager
1. Right-click project → **Manage NuGet Packages**
2. **Updates** tab
3. Find **ETCStorageHelper**
4. Click **Update** → Select **1.0.2**

### Option 2: Package Manager Console
```powershell
Update-Package ETCStorageHelper -Version 1.0.2
```

### Option 3: Edit .csproj Directly
```xml
<PackageReference Include="ETCStorageHelper" Version="1.0.2" />
```

## ✅ Breaking Changes

**None** - This is a backward-compatible update. All existing code continues to work.

## 📝 Full Changelog

### v1.0.2 (December 9, 2025) - Complete Delete Operations
- ✨ Added `ETCDirectory.Delete()` method
- ✨ Added `ETCFileAsync.DeleteAsync()` method  
- ✨ Added `SharePointClient.DeleteFolderAsync()` method
- 📚 Enhanced documentation with sync/async behavior clarification
- 📚 Updated README with "Blocks?" indicators in API tables
- 🐛 Fixed: Directory delete was not possible before

### v1.0.1 (December 9, 2025) - IntelliSense Support
- ✨ Added XML documentation for IntelliSense support
- ✅ Confirmed .NET Framework 4.6 compatibility

### v1.0.0 (Initial Release)
- ✨ File operations (ETCFile)
- ✨ Directory operations (ETCDirectory)
- ✨ Path utilities (ETCPath)
- ✨ Azure AD authentication
- ✨ SharePoint Graph API integration
- ✨ Large file support with chunking
- ✨ Automatic retry with exponential backoff
- ✨ Mandatory audit logging

## 🚀 Testing the New Features

After updating, test the new delete functionality:

```csharp
using ETCStorageHelper;

// Test directory deletion
var site = SharePointSite.FromConfig("Commercial", "ETCStorage.Commercial", 
    "user123", "John Doe", "TestApp");

// Create test directory
ETCDirectory.CreateDirectory("TestFolder/SubFolder", site);

// Delete it (synchronous)
ETCDirectory.Delete("TestFolder/SubFolder", site);

// Delete with contents (recursive)
ETCDirectory.Delete("TestFolder", site, recursive: true);

Console.WriteLine("✓ Directory delete works!");

// Test async file deletion
var handle = ETCFileAsync.DeleteAsync(
    "OldFile.txt",
    site,
    onSuccess: p => Console.WriteLine($"✓ Deleted: {p}"),
    onError: (p, ex) => Console.WriteLine($"✗ Failed: {ex.Message}")
);

Console.WriteLine($"✓ Async delete queued: {handle.UploadId}");
```

## 📍 Package Location

**New Package:** `C:\dev\etc\etc-virtual-drive-consumer\ETCStorageHelper.1.0.2.nupkg`

**Distribution Folder:** `C:\dev\etc\etc-virtual-drive\distribute\packages\`

## 📞 Support

Questions about the new delete methods or sync/async behavior?
- Check the updated README.md
- Review code examples above
- Contact: Pablo (ETC Development Team)

---

**Released:** December 9, 2025  
**Package Size:** 97.24 KB  
**Compatibility:** .NET Framework 4.6+


