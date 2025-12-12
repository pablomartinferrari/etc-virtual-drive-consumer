# ETCStorageHelper Test App — Final Guide

This README shows:
- How to initialize two `SharePointSite` clients (Commercial and CUI) with separate credentials.
- How to choose which client to use based on simple runtime logic.
- A catalog of the methods exercised in this app across `ETCFile`, `ETCDirectory`, and `ETCPath`.

## Initialize Two Clients

Ensure your `App.config` contains two credential sets, e.g.:

```xml
<appSettings>
  <!-- Commercial site -->
  <add key="ETCStorage.Commercial.TenantId" value="..."/>
  <add key="ETCStorage.Commercial.ClientId" value="..."/>
  <add key="ETCStorage.Commercial.ClientSecret" value="..."/>
  <add key="ETCStorage.Commercial.SiteUrl" value="https://<tenant>.sharepoint.com/sites/<site>"/>
  <add key="ETCStorage.Commercial.LibraryName" value="Documents"/>

  <!-- CUI site -->
  <add key="ETCStorage.CUI.TenantId" value="..."/>
  <add key="ETCStorage.CUI.ClientId" value="..."/>
  <add key="ETCStorage.CUI.ClientSecret" value="..."/>
  <add key="ETCStorage.CUI.SiteUrl" value="https://<tenant>.sharepoint.com/sites/<cui-site>"/>
  <add key="ETCStorage.CUI.LibraryName" value="Documents"/>

  <!-- Test runtime info -->
  <add key="Test.UserId" value="{APPLICATION USER ID}"/>
  <add key="Test.UserName" value="Your Name"/>
  <add key="Test.ApplicationName" value="Your App Name"/>
  <add key="Test.BasePath" value="G"/>
</appSettings>
```

Create two `SharePointSite` instances:

```csharp
using ETCStorageHelper;

// Common runtime info (optional)
var userId = System.Configuration.ConfigurationManager.AppSettings["Test.UserId"];
var userName = System.Configuration.ConfigurationManager.AppSettings["Test.UserName"];
var applicationName = System.Configuration.ConfigurationManager.AppSettings["Test.ApplicationName"];

// Commercial client
var commercialSite = SharePointSite.FromConfig(
    name: "Commercial",
    configPrefix: "ETCStorage.Commercial",
    userId: userId,
    userName: userName,
    applicationName: applicationName
);

// CUI client
var cuiSite = SharePointSite.FromConfig(
    name: "CUI",
    configPrefix: "ETCStorage.CUI",
    userId: userId,
    userName: userName,
    applicationName: applicationName
);
```

## Choose Client at Runtime

Example of simple logic to decide which site to use (replace with your own rules):

```csharp
SharePointSite ResolveSiteForPath(string logicalPath)
{
    // Dummy logic: routes any path under "ClientA" to Commercial; others to CUI
    if (logicalPath.StartsWith("ClientA/", StringComparison.OrdinalIgnoreCase))
        return commercialSite;
    return cuiSite;
}

string logicalPath = ETCPath.Combine("ClientA", "2025", "Reports", "annual-report.pdf");
var site = ResolveSiteForPath(logicalPath);

// Use selected site to perform operations
ETCFile.WriteAllText(logicalPath, "Hello", site);
string url = ETCFile.GetFileUrl(logicalPath, site);
Console.WriteLine($"File URL: {url}");
```

You can also choose based on file extension, sensitivity labels, or a configuration flag.

## TLS 1.2 and Proxy Initialization (Program.cs)

For corporate environments, enable TLS 1.2 and use system proxy credentials early in your app startup. Add this near the top of `Program.Main` before any calls to the helper:

```csharp
using System.Net;

// Ensure modern TLS and corporate proxy support before any HTTP calls
ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
if (WebRequest.DefaultWebProxy != null)
{
        WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
}
```

Optional App.config additions to use default proxy creds and enable network tracing (writes `network.log`):

```xml
<configuration>
    <system.net>
        <defaultProxy useDefaultCredentials="true" />
    </system.net>
    <system.diagnostics>
        <sources>
            <source name="System.Net" tracemode="includehex" maxdatasize="1024">
                <listeners>
                    <add name="System.Net" />
                </listeners>
            </source>
            <source name="System.Net.Http">
                <listeners>
                    <add name="System.Net" />
                </listeners>
            </source>
        </sources>
        <switches>
            <add name="System.Net" value="Verbose" />
            <add name="System.Net.Http" value="Verbose" />
        </switches>
        <sharedListeners>
            <add name="System.Net" type="System.Diagnostics.TextWriterTraceListener" initializeData="network.log" />
        </sharedListeners>
        <trace autoflush="true" />
    </system.diagnostics>
</configuration>
```

## API Catalog (as used by this app)

The following methods are actively exercised in this test app. Refer to the library docs for any additional methods not listed here.

### `ETCFile`
- **WriteAllText(path, text, site):** Uploads/overwrites a text file.
- **WriteAllBytes(path, bytes, site):** Uploads/overwrites a binary file.
- **ReadAllText(path, site):** Reads a text file.
- **ReadAllBytes(path, site):** Reads a binary file.
- **Exists(path, site):** Checks file existence.
- **Copy(sourcePath, destPath, site):** Copies a file.
- **GetFileUrl(path, site):** Returns the SharePoint URL for a file.

### `ETCDirectory`
- **CreateDirectory(path, site):** Creates folders (recursively as needed).
- **Exists(path, site):** Checks folder existence.
- **GetFiles(path, site):** Lists files within a folder.
- **GetDirectories(path, site):** Lists subfolders.
- **GetFolderUrl(path, site):** Returns the SharePoint URL for a folder.
- **Delete(path, site, recursive):** Deletes a folder; set `recursive` to remove all contents.

### `ETCPath`
- **Combine(...parts):** Combines segments using forward slashes.
- **GetDirectoryName(path):** Returns the directory portion.
- **GetFileName(path):** Returns the file name.
- **GetExtension(path):** Returns the file extension (including dot).
- **GetFileNameWithoutExtension(path):** Returns the file name without extension.

### `ETCFileAsync` (if using large-file scenarios)
- Methods are covered in the test app’s `ETCFileAsyncTests` and typically mirror sync operations for large content. Use where async flows are preferred.


## Notes
- This app enables TLS 1.2 and uses system proxy credentials at startup to improve connectivity in corporate environments.
- App.config supports separate credentials per site via `configPrefix` (e.g., `ETCStorage.Commercial`, `ETCStorage.CUI`).

