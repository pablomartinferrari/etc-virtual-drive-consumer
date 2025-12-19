# ETCStorageHelper Consumer Applications

This repository contains consumer applications that demonstrate how to use the `ETCStorageHelper` library to interact with SharePoint sites across different cloud environments (Commercial and GCC High).

## Applications

### 1. ETCStorageHelper.WinFormsDemo
A Windows Forms demonstration application that provides a GUI for testing SharePoint operations across multiple environments.

### 2. ETCStorageHelper.TestApp
A console application for automated testing of the ETCStorageHelper library functionality.

---

## WinFormsDemo Application

### Overview

The WinFormsDemo application demonstrates how to:
- Configure and connect to multiple SharePoint environments (Commercial and GCC High)
- Perform file and directory operations (create, read, write, delete)
- Switch between environments at runtime
- Test connections and verify permissions

### How It Works

The application initializes two separate `SharePointSite` instances at startup - one for Commercial and one for GCC High. Users can switch between environments using radio buttons in the UI, and all operations are performed against the currently selected site.

### Instantiating Different Libraries (Commercial vs GCC High)

The application uses the `SharePointSite.FromConfig()` method to create site configurations from `App.config`. Each environment has its own configuration section with a unique prefix.

#### Step 1: Configure App.config

Add configuration sections for each environment you want to support:

```xml
<appSettings>
  <!-- ============================================================ -->
  <!-- GCC High SharePoint Site Configuration                       -->
  <!-- ============================================================ -->
  <add key="ETCStorage.GCCHigh.TenantId" value="your-gcc-high-tenant-id"/>
  <add key="ETCStorage.GCCHigh.ClientId" value="your-gcc-high-client-id"/>
  <add key="ETCStorage.GCCHigh.ClientSecret" value="your-gcc-high-client-secret"/>
  <add key="ETCStorage.GCCHigh.SiteUrl" value="https://yourtenant.sharepoint.us/sites/YourSite"/>
  <add key="ETCStorage.GCCHigh.LibraryName" value="Documents"/>
  <add key="ETCStorage.GCCHigh.Environment" value="GCCHigh"/>

  <!-- ============================================================ -->
  <!-- Commercial SharePoint Site Configuration                     -->
  <!-- ============================================================ -->
  <add key="ETCStorage.Commercial.TenantId" value="your-commercial-tenant-id"/>
  <add key="ETCStorage.Commercial.ClientId" value="your-commercial-client-id"/>
  <add key="ETCStorage.Commercial.ClientSecret" value="your-commercial-client-secret"/>
  <add key="ETCStorage.Commercial.SiteUrl" value="https://yourtenant.sharepoint.com/sites/YourSite"/>
  <add key="ETCStorage.Commercial.LibraryName" value="Documents"/>
  <add key="ETCStorage.Commercial.Environment" value="Commercial"/>
</appSettings>
```

**Important Configuration Notes:**
- **Environment**: Must be set to `"Commercial"`, `"GCCHigh"`, or `"DoD"` to use the correct authentication endpoints
- **SiteUrl Format**: 
  - Commercial: `https://tenant.sharepoint.com/sites/SiteName` or `https://tenant.sharepoint.com/` (root site)
  - GCC High: `https://tenant.sharepoint.us/sites/SiteName`
- **LibraryName**: Typically `"Documents"` for modern team sites, or `"Shared Documents"` for communication sites

#### Step 2: Initialize Site Configurations in Code

In your application startup code (e.g., `MainForm.cs`), create `SharePointSite` instances using the configuration prefixes:

```csharp
using ETCStorageHelper;

public partial class MainForm : Form
{
    private SharePointSite _gccHighSite;
    private SharePointSite _commercialSite;
    
    private void InitializeSites()
    {
        // Initialize GCC High site
        _gccHighSite = SharePointSite.FromConfig(
            name: "GCCHigh",
            configPrefix: "ETCStorage.GCCHigh",  // Matches App.config prefix
            userId: Environment.UserName,
            userName: Environment.UserName,
            applicationName: "WinForms Demo"
        );
        
        // Initialize Commercial site
        _commercialSite = SharePointSite.FromConfig(
            name: "Commercial",
            configPrefix: "ETCStorage.Commercial",  // Matches App.config prefix
            userId: Environment.UserName,
            userName: Environment.UserName,
            applicationName: "WinForms Demo"
        );
    }
}
```

#### Step 3: Select Which Site to Use

The application uses a property to determine the current site based on user selection:

```csharp
private SharePointSite CurrentSite => rbGCCHigh.Checked ? _gccHighSite : _commercialSite;
```

#### Step 4: Use the Selected Site

All operations use the `CurrentSite` instance:

```csharp
// Create a directory
ETCDirectory.CreateDirectory("MyFolder/SubFolder", CurrentSite);

// Write a file
ETCFile.WriteAllText("MyFolder/file.txt", "Hello World", CurrentSite);

// Read a file
string content = ETCFile.ReadAllText("MyFolder/file.txt", CurrentSite);
```

### Key Concepts

#### Configuration Prefix Pattern

The `configPrefix` parameter tells `SharePointSite.FromConfig()` which section of `App.config` to read from:

- `configPrefix: "ETCStorage.GCCHigh"` → Reads keys like `ETCStorage.GCCHigh.TenantId`, `ETCStorage.GCCHigh.ClientId`, etc.
- `configPrefix: "ETCStorage.Commercial"` → Reads keys like `ETCStorage.Commercial.TenantId`, `ETCStorage.Commercial.ClientId`, etc.

This allows you to define multiple environments in the same `App.config` file.

#### Environment-Specific Endpoints

The `Environment` setting in `App.config` determines which authentication and Graph API endpoints are used:

- **Commercial**: 
  - Login: `https://login.microsoftonline.com`
  - Graph: `https://graph.microsoft.com`
- **GCC High**: 
  - Login: `https://login.microsoftonline.us`
  - Graph: `https://graph.microsoft.us`
- **DoD**: 
  - Login: `https://login.microsoftonline.us`
  - Graph: `https://dod-graph.microsoft.us`

**Important**: This feature requires ETCStorageHelper version 1.0.7 or later. Version 1.0.6 only supports Commercial endpoints.

### Features

- **Environment Switching**: Toggle between Commercial and GCC High at runtime
- **Connection Testing**: Test button verifies authentication, permissions, and write capabilities
- **File Operations**: Create directories, read/write files, get URLs
- **Error Handling**: Detailed error messages with troubleshooting hints
- **Logging**: Real-time operation logging in the UI

### Prerequisites

1. **ETCStorageHelper NuGet Package**: Version 1.0.7 or later (for multi-environment support)
2. **Azure AD App Registrations**: 
   - One app registration per environment (Commercial and GCC High)
   - Each must have `Sites.ReadWrite.All` (Application permission) with admin consent
3. **SharePoint Sites**: 
   - Sites must exist and be accessible
   - App registrations must have access to the sites

### Troubleshooting

#### "Unauthorized" Error
- Verify `Sites.ReadWrite.All` permission is granted and has admin consent
- Check that the Client Secret hasn't expired
- Verify the Tenant ID matches the tenant that owns the SharePoint site

#### "Invalid hostname for this tenancy" Error
- The SharePoint site URL doesn't belong to the tenant specified in `TenantId`
- Verify the correct Tenant ID for your SharePoint site
- Check that the Site URL is correct

#### "Site not found" Error
- Verify the site exists and the URL format is correct
- For modern team sites, use `/sites/SiteName` format
- For root site, use just the tenant URL with trailing slash

---

## TestApp Application

See `ETCStorageHelper.TestApp/README.md` for details on the console test application.

---

## Getting Azure AD Credentials

### For Commercial Environment

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** → **App registrations**
3. Find or create your app registration
4. Copy the **Directory (Tenant) ID** and **Application (Client) ID**
5. Go to **Certificates & secrets** → Create a new client secret
6. Copy the secret value immediately (it won't be shown again)

### For GCC High Environment

1. Go to [Azure Portal for GCC High](https://portal.azure.us)
2. Follow the same steps as above
3. **Important**: Make sure you're in the GCC High portal, not the commercial one

### Required Permissions

Both app registrations need:
- **Microsoft Graph** → `Sites.ReadWrite.All` (Application permission)
- **Admin consent** must be granted

---

## Version Requirements

- **ETCStorageHelper 1.0.7+**: Required for GCC High and DoD support
- **ETCStorageHelper 1.0.6**: Only supports Commercial environment

---

## Additional Resources

- [ETCStorageHelper Library Documentation](../../etc-virtual-drive/README.md)
- [Configuration Guide](../../etc-virtual-drive/src/CONFIGURATION.md)
- PowerShell test scripts in `../../tools/`:
  - `Test-CommercialAuth.ps1` - Test Commercial credentials
  - `Test-GCCHighAuth.ps1` - Test GCC High credentials
