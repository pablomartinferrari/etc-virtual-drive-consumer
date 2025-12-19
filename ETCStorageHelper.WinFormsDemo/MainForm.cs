using System;
using System.Drawing;
using System.Windows.Forms;
using ETCStorageHelper;

namespace ETCStorageHelper.WinFormsDemo
{
    public partial class MainForm : Form
    {
        // SharePoint site configurations - one for each environment
        private SharePointSite _gccHighSite;
        private SharePointSite _commercialSite;
        
        // Currently selected site
        private SharePointSite CurrentSite => rbGCCHigh.Checked ? _gccHighSite : _commercialSite;

        public MainForm()
        {
            InitializeComponent();
            InitializeSites();
            UpdateStatusLabel();
        }

        /// <summary>
        /// Initialize both SharePoint site configurations from App.config
        /// </summary>
        private void InitializeSites()
        {
            try
            {
                // Initialize GCC High site
                _gccHighSite = SharePointSite.FromConfig(
                    name: "GCCHigh",
                    configPrefix: "ETCStorage.GCCHigh",
                    userId: Environment.UserName,
                    userName: Environment.UserName,
                    applicationName: "WinForms Demo"
                );
                Log("✓ GCC High site configuration loaded");
                Log($"  Site URL: {_gccHighSite.SiteUrl}");
            }
            catch (Exception ex)
            {
                Log($"✗ Failed to load GCC High config: {ex.Message}");
                rbGCCHigh.Enabled = false;
            }

            try
            {
                // Initialize Commercial site
                _commercialSite = SharePointSite.FromConfig(
                    name: "Commercial",
                    configPrefix: "ETCStorage.Commercial",
                    userId: Environment.UserName,
                    userName: Environment.UserName,
                    applicationName: "WinForms Demo"
                );
                Log("✓ Commercial site configuration loaded");
                Log($"  Site URL: {_commercialSite.SiteUrl}");
            }
            catch (Exception ex)
            {
                Log($"✗ Failed to load Commercial config: {ex.Message}");
                Log($"  Stack: {ex.StackTrace}");
                rbCommercial.Enabled = false;
            }

            Log("");
            Log("Ready! Select an environment and try the operations.");
            Log("─────────────────────────────────────────────────────");
        }

        /// <summary>
        /// Update status label when environment changes
        /// </summary>
        private void Environment_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            if (rbGCCHigh.Checked)
            {
                lblStatus.Text = "Environment: GCC High (graph.microsoft.us)";
                lblStatus.ForeColor = Color.DarkBlue;
            }
            else
            {
                lblStatus.Text = "Environment: Commercial (graph.microsoft.com)";
                lblStatus.ForeColor = Color.DarkGreen;
            }
        }

        /// <summary>
        /// Create a directory in SharePoint
        /// </summary>
        private void btnCreateDirectory_Click(object sender, EventArgs e)
        {
            var folderPath = txtFolderPath.Text.Trim();
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please enter a folder path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunOperation("Create Directory", () =>
            {
                Log($"Creating directory: {folderPath}");
                Log($"Site URL: {CurrentSite.SiteUrl}");
                
                ETCDirectory.CreateDirectory(folderPath, CurrentSite);
                
                Log($"✓ Directory created successfully!");
            });
        }

        /// <summary>
        /// Get the SharePoint URL for a directory
        /// </summary>
        private void btnGetDirectoryUrl_Click(object sender, EventArgs e)
        {
            var folderPath = txtFolderPath.Text.Trim();
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please enter a folder path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunOperation("Get Directory URL", () =>
            {
                Log($"Getting URL for: {folderPath}");
                Log($"Site URL: {CurrentSite.SiteUrl}");
                
                var url = ETCDirectory.GetFolderUrl(folderPath, CurrentSite);
                
                Log($"✓ Directory URL:");
                Log($"  {url}");
                
                // Copy to clipboard
                Clipboard.SetText(url);
                Log("  (URL copied to clipboard)");
            });
        }

        /// <summary>
        /// Write a file to SharePoint
        /// </summary>
        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            var filePath = txtFileName.Text.Trim();
            var content = txtFileContent.Text.Replace("{timestamp}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter a file path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunOperation("Write File", () =>
            {
                Log($"Writing file: {filePath}");
                Log($"Site URL: {CurrentSite.SiteUrl}");
                Log($"Content length: {content.Length} characters");
                
                ETCFile.WriteAllText(filePath, content, CurrentSite);
                
                Log($"✓ File written successfully!");
                
                // Get and display the file URL
                var url = ETCFile.GetFileUrl(filePath, CurrentSite);
                Log($"  File URL: {url}");
            });
        }

        /// <summary>
        /// Read a file from SharePoint
        /// </summary>
        private void btnReadFile_Click(object sender, EventArgs e)
        {
            var filePath = txtFileName.Text.Trim();
            
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please enter a file path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunOperation("Read File", () =>
            {
                Log($"Reading file: {filePath}");
                Log($"Site URL: {CurrentSite.SiteUrl}");
                
                var content = ETCFile.ReadAllText(filePath, CurrentSite);
                
                Log($"✓ File read successfully!");
                Log($"  Content length: {content.Length} characters");
                Log($"  Content:");
                Log("  ─────────────────────────────────────");
                foreach (var line in content.Split('\n'))
                {
                    Log($"  {line.TrimEnd('\r')}");
                }
                Log("  ─────────────────────────────────────");
            });
        }

        /// <summary>
        /// Run an operation with error handling and UI updates
        /// </summary>
        private void RunOperation(string operationName, Action operation)
        {
            Log("");
            Log($"▶ {operationName}...");
            
            // Disable buttons during operation
            SetButtonsEnabled(false);
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                operation();
            }
            catch (Exception ex)
            {
                Log($"✗ Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Log($"  Inner: {ex.InnerException.Message}");
                }
                
                // Show detailed error information
                var errorDetails = $"Operation: {operationName}\n";
                errorDetails += $"Site URL: {CurrentSite?.SiteUrl}\n";
                errorDetails += $"Error: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    errorDetails += $"\n\nInner Exception: {ex.InnerException.Message}";
                }
                
                // Check for common issues
                if (ex.Message.Contains("Unauthorized") || ex.Message.Contains("401"))
                {
                    errorDetails += "\n\n⚠ Possible causes:";
                    errorDetails += "\n  • Sites.ReadWrite.All permission not granted";
                    errorDetails += "\n  • Admin consent not granted";
                    errorDetails += "\n  • Wrong app registration credentials";
                }
                else if (ex.Message.Contains("NotFound") || ex.Message.Contains("404"))
                {
                    errorDetails += "\n\n⚠ Possible causes:";
                    errorDetails += "\n  • Wrong Site URL";
                    errorDetails += "\n  • Site doesn't exist";
                    errorDetails += "\n  • Wrong Library Name";
                }
                
                Log($"  Full error details logged above");
                MessageBox.Show(errorDetails, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetButtonsEnabled(true);
                Cursor = Cursors.Default;
                Log("─────────────────────────────────────────────────────");
            }
        }

        private void SetButtonsEnabled(bool enabled)
        {
            btnCreateDirectory.Enabled = enabled;
            btnGetDirectoryUrl.Enabled = enabled;
            btnWriteFile.Enabled = enabled;
            btnReadFile.Enabled = enabled;
        }

        /// <summary>
        /// Log a message to the output textbox
        /// </summary>
        private void Log(string message)
        {
            txtOutput.AppendText($"{message}\r\n");
            txtOutput.SelectionStart = txtOutput.Text.Length;
            txtOutput.ScrollToCaret();
        }

        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        /// <summary>
        /// Test connection to the currently selected SharePoint site
        /// </summary>
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (CurrentSite == null)
            {
                MessageBox.Show("Site configuration not loaded. Check App.config.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunOperation("Test Connection", () =>
            {
                Log($"Testing connection to: {CurrentSite.SiteUrl}");
                Log($"Tenant ID: {CurrentSite.TenantId}");
                Log($"Client ID: {CurrentSite.ClientId}");
                Log("");

                // Step 1: Test site access (this will also test authentication)
                Log("Step 1: Testing site access and authentication...");
                try
                {
                    var siteExists = ETCDirectory.Exists("", CurrentSite);
                    Log($"  ✓ Site access successful!");
                    Log($"  (Authentication passed - token acquired successfully)");
                }
                catch (Exception ex)
                {
                    Log($"  ✗ Site access failed: {ex.Message}");
                    if (ex.Message.Contains("Unauthorized") || ex.Message.Contains("401"))
                    {
                        Log($"  → This indicates an authentication or permissions issue");
                    }
                    throw;
                }

                Log("");

                // Step 2: Test library access
                Log("Step 2: Testing document library access...");
                try
                {
                    var files = ETCDirectory.GetFiles("", CurrentSite);
                    Log($"  ✓ Library access successful!");
                    Log($"  Found {files.Length} files in root");
                }
                catch (Exception ex)
                {
                    Log($"  ✗ Library access failed: {ex.Message}");
                    throw;
                }

                Log("");

                // Step 3: Test write capability (create a test folder)
                Log("Step 3: Testing write capability...");
                try
                {
                    var testFolder = $"WinFormsTest_{DateTime.Now:yyyyMMddHHmmss}";
                    ETCDirectory.CreateDirectory(testFolder, CurrentSite);
                    Log($"  ✓ Write capability confirmed!");
                    Log($"  Created test folder: {testFolder}");
                    
                    // Clean up - delete the test folder
                    try
                    {
                        ETCDirectory.Delete(testFolder, CurrentSite);
                        Log($"  (Test folder cleaned up)");
                    }
                    catch
                    {
                        Log($"  (Note: Test folder '{testFolder}' was created but not deleted)");
                    }
                }
                catch (Exception ex)
                {
                    Log($"  ✗ Write capability test failed: {ex.Message}");
                    throw;
                }

                Log("");
                Log("✓ All connection tests passed!");
            });
        }
    }
}
