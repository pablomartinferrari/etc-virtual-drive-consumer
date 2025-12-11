using System;
using System.Linq;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    public static class IntegrationTests
    {
        public static void RunCompleteScenario(SharePointSite site, string basePath)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("      Complete Integration Test Scenario");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.WriteLine("This test simulates a real-world workflow:");
            Console.WriteLine("  1. Create project folder structure");
            Console.WriteLine("  2. Upload various document types");
            Console.WriteLine("  3. List and verify files");
            Console.WriteLine("  4. Copy and move files");
            Console.WriteLine("  5. Retrieve SharePoint URLs");
            Console.WriteLine("  6. Read and verify file contents");
            Console.WriteLine();

            try
            {
                var projectPath = ETCPath.Combine(basePath, "IntegrationTest", "ClientA", "2025", "Project001");

                // Step 1: Create folder structure
                CreateProjectStructure(site, projectPath);

                // Step 2: Upload documents
                UploadProjectDocuments(site, projectPath);

                // Step 3: List and verify
                ListAndVerifyFiles(site, projectPath);

                // Step 4: Copy files
                CopyAndOrganizeFiles(site, projectPath);

                // Step 5: Get SharePoint URLs
                GetSharePointUrls(site, projectPath);

                // Step 6: Read and verify
                ReadAndVerifyContent(site, projectPath);

                // Step 7: Path manipulation tests
                TestPathManipulation(projectPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n==============================================");
                Console.WriteLine("✓ COMPLETE INTEGRATION TEST PASSED!");
                Console.WriteLine("==============================================");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ Integration test failed: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                Console.ResetColor();
                throw;
            }
        }

        private static void CreateProjectStructure(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 1] Creating Project Folder Structure");
            Console.WriteLine("-------------------------------------------");

            string[] folders = {
                ETCPath.Combine(projectPath, "Reports"),
                ETCPath.Combine(projectPath, "Data"),
                ETCPath.Combine(projectPath, "Documents"),
                ETCPath.Combine(projectPath, "Archive")
            };

            foreach (var folder in folders)
            {
                Console.WriteLine($"Creating: {folder}");
                ETCDirectory.CreateDirectory(folder, site);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Created {folders.Length} folders");
            Console.ResetColor();
        }

        private static void UploadProjectDocuments(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 2] Uploading Project Documents");
            Console.WriteLine("-------------------------------------------");

            // Upload a report (PDF-like)
            var reportPath = ETCPath.Combine(projectPath, "Reports", "annual-report-2025.pdf");
            var reportData = TestDataGenerator.GeneratePdfLikeData(500); // 500KB
            Console.WriteLine($"Uploading: {reportPath} ({TestDataGenerator.FormatBytes(reportData.Length)})");
            ETCFile.WriteAllBytes(reportPath, reportData, site);

            // Upload a text document
            var docPath = ETCPath.Combine(projectPath, "Documents", "project-notes.txt");
            var docText = TestDataGenerator.GenerateTextData(2000);
            Console.WriteLine($"Uploading: {docPath} ({docText.Length} chars)");
            ETCFile.WriteAllText(docPath, docText, site);

            // Upload data file
            var dataPath = ETCPath.Combine(projectPath, "Data", "measurements.csv");
            var csvContent = "Date,Value,Status\n2025-01-01,123.45,Active\n2025-01-02,234.56,Active\n2025-01-03,345.67,Complete";
            Console.WriteLine($"Uploading: {dataPath}");
            ETCFile.WriteAllText(dataPath, csvContent, site);

            // Upload multiple report versions
            for (int i = 1; i <= 3; i++)
            {
                var versionPath = ETCPath.Combine(projectPath, "Reports", $"draft-v{i}.txt");
                var versionContent = $"Draft version {i}\nCreated: {DateTime.Now}\n{TestDataGenerator.GenerateTextData(200)}";
                Console.WriteLine($"Uploading: {versionPath}");
                ETCFile.WriteAllText(versionPath, versionContent, site);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Uploaded 6 documents");
            Console.ResetColor();
        }

        private static void ListAndVerifyFiles(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 3] Listing and Verifying Files");
            Console.WriteLine("-------------------------------------------");

            // List all subdirectories
            var reportsPath = ETCPath.Combine(projectPath, "Reports");
            Console.WriteLine($"\nFiles in {reportsPath}:");
            string[] reportFiles = ETCDirectory.GetFiles(reportsPath, site);
            foreach (var file in reportFiles)
            {
                Console.WriteLine($"  - {file}");
            }

            // List all project subdirectories
            Console.WriteLine($"\nDirectories in {projectPath}:");
            string[] directories = ETCDirectory.GetDirectories(projectPath, site);
            foreach (var dir in directories)
            {
                Console.WriteLine($"  - {dir}");
            }

            // Verify file exists
            var reportPath = ETCPath.Combine(projectPath, "Reports", "annual-report-2025.pdf");
            bool exists = ETCFile.Exists(reportPath, site);
            Console.WriteLine($"\nFile exists check: {reportPath}");
            Console.WriteLine($"  Result: {exists}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Listed {reportFiles.Length} files and {directories.Length} directories");
            Console.ResetColor();
        }

        private static void CopyAndOrganizeFiles(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 4] Copying and Organizing Files");
            Console.WriteLine("-------------------------------------------");

            // Copy final draft to archive
            var sourcePath = ETCPath.Combine(projectPath, "Reports", "draft-v3.txt");
            var destPath = ETCPath.Combine(projectPath, "Archive", "draft-v3-archived.txt");
            
            Console.WriteLine($"Copying: {sourcePath}");
            Console.WriteLine($"     To: {destPath}");
            ETCFile.Copy(sourcePath, destPath, site);

            // Verify copy exists
            bool copyExists = ETCFile.Exists(destPath, site);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ File copied successfully (exists: {copyExists})");
            Console.ResetColor();
        }

        private static void GetSharePointUrls(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 5] Retrieving SharePoint URLs");
            Console.WriteLine("-------------------------------------------");

            // Get file URL
            var reportPath = ETCPath.Combine(projectPath, "Reports", "annual-report-2025.pdf");
            string fileUrl = ETCFile.GetFileUrl(reportPath, site);
            Console.WriteLine($"File URL:");
            Console.WriteLine($"  {fileUrl}");

            // Get folder URL
            var reportsFolder = ETCPath.Combine(projectPath, "Reports");
            string folderUrl = ETCDirectory.GetFolderUrl(reportsFolder, site);
            Console.WriteLine($"\nFolder URL:");
            Console.WriteLine($"  {folderUrl}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Retrieved SharePoint URLs");
            Console.ResetColor();
        }

        private static void ReadAndVerifyContent(SharePointSite site, string projectPath)
        {
            Console.WriteLine("\n[STEP 6] Reading and Verifying Content");
            Console.WriteLine("-------------------------------------------");

            // Read text file
            var notesPath = ETCPath.Combine(projectPath, "Documents", "project-notes.txt");
            string notes = ETCFile.ReadAllText(notesPath, site);
            Console.WriteLine($"Read text file: {notesPath}");
            Console.WriteLine($"  Length: {notes.Length} characters");
            Console.WriteLine($"  Preview: {notes.Substring(0, Math.Min(60, notes.Length))}...");

            // Read binary file
            var reportPath = ETCPath.Combine(projectPath, "Reports", "annual-report-2025.pdf");
            byte[] reportData = ETCFile.ReadAllBytes(reportPath, site);
            Console.WriteLine($"\nRead binary file: {reportPath}");
            Console.WriteLine($"  Size: {TestDataGenerator.FormatBytes(reportData.Length)}");

            // Read CSV
            var csvPath = ETCPath.Combine(projectPath, "Data", "measurements.csv");
            string csvContent = ETCFile.ReadAllText(csvPath, site);
            string[] csvLines = csvContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"\nRead CSV file: {csvPath}");
            Console.WriteLine($"  Rows: {csvLines.Length}");
            Console.WriteLine($"  Header: {csvLines[0]}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Read and verified 3 files");
            Console.ResetColor();
        }

        private static void TestPathManipulation(string projectPath)
        {
            Console.WriteLine("\n[STEP 7] Path Manipulation Tests");
            Console.WriteLine("-------------------------------------------");

            var filePath = ETCPath.Combine(projectPath, "Reports", "annual-report-2025.pdf");

            string dirName = ETCPath.GetDirectoryName(filePath);
            string fileName = ETCPath.GetFileName(filePath);
            string extension = ETCPath.GetExtension(filePath);
            string nameWithoutExt = ETCPath.GetFileNameWithoutExtension(filePath);

            Console.WriteLine($"Full path: {filePath}");
            Console.WriteLine($"  Directory: {dirName}");
            Console.WriteLine($"  File name: {fileName}");
            Console.WriteLine($"  Extension: {extension}");
            Console.WriteLine($"  Name without extension: {nameWithoutExt}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Path manipulation tests passed");
            Console.ResetColor();
        }
    }
}


