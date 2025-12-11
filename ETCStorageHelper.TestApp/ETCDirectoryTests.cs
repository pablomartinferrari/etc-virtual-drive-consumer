using System;
using System.Linq;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    public static class ETCDirectoryTests
    {
        public static void RunAllTests(SharePointSite site, string basePath)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("       Testing ETCDirectory Methods");
            Console.WriteLine("==============================================");

            try
            {
                TestCreateDirectory(site, basePath);
                TestExists(site, basePath);
                
                // Create some test files for directory listing
                SetupDirectoryTestData(site, basePath);
                
                TestGetFiles(site, basePath);
                TestGetDirectories(site, basePath);
                TestGetFolderUrl(site, basePath);
                TestDeleteNonRecursive(site, basePath);
                TestDeleteRecursive(site, basePath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ All ETCDirectory tests completed successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ ETCDirectory tests failed: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private static void TestCreateDirectory(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.CreateDirectory");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "Level1", "Level2", "Level3");
            
            Console.WriteLine($"Creating nested directory: {testPath}");
            var startTime = DateTime.Now;
            
            ETCDirectory.CreateDirectory(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ CreateDirectory succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.ResetColor();
        }

        private static void TestExists(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.Exists");
            Console.WriteLine("-------------------------------------------");
            
            var existingPath = ETCPath.Combine(basePath, "DirectoryTests", "Level1");
            var nonExistingPath = ETCPath.Combine(basePath, "DirectoryTests", "DoesNotExist");
            
            Console.WriteLine($"Checking if directory exists: {existingPath}");
            bool exists1 = ETCDirectory.Exists(existingPath, site);
            
            Console.WriteLine($"Checking if directory exists: {nonExistingPath}");
            bool exists2 = ETCDirectory.Exists(nonExistingPath, site);
            
            if (exists1 && !exists2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Exists check succeeded");
                Console.WriteLine($"  {existingPath}: {exists1}");
                Console.WriteLine($"  {nonExistingPath}: {exists2}");
                Console.ResetColor();
            }
            else
            {
                throw new Exception($"Exists check failed: existing={exists1}, non-existing={exists2}");
            }
        }

        private static void SetupDirectoryTestData(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[SETUP] Creating test files and directories...");
            
            var dirPath = ETCPath.Combine(basePath, "DirectoryTests", "ListingTest");
            
            // Create some files in the directory
            for (int i = 1; i <= 3; i++)
            {
                var filePath = ETCPath.Combine(dirPath, $"file{i}.txt");
                ETCFile.WriteAllText(filePath, $"Test file {i}", site);
            }
            
            // Create some subdirectories
            for (int i = 1; i <= 2; i++)
            {
                var subDirPath = ETCPath.Combine(dirPath, $"SubDir{i}");
                ETCDirectory.CreateDirectory(subDirPath, site);
                
                // Add a file to each subdirectory
                var subFilePath = ETCPath.Combine(subDirPath, "subfile.txt");
                ETCFile.WriteAllText(subFilePath, $"File in SubDir{i}", site);
            }
            
            Console.WriteLine("✓ Test data created");
        }

        private static void TestGetFiles(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.GetFiles");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "ListingTest");
            
            Console.WriteLine($"Getting files from: {testPath}");
            var startTime = DateTime.Now;
            
            string[] files = ETCDirectory.GetFiles(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ GetFiles succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  Found {files.Length} files:");
            foreach (var file in files)
            {
                Console.WriteLine($"    - {file}");
            }
            Console.ResetColor();
        }

        private static void TestGetDirectories(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.GetDirectories");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "ListingTest");
            
            Console.WriteLine($"Getting directories from: {testPath}");
            var startTime = DateTime.Now;
            
            string[] directories = ETCDirectory.GetDirectories(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ GetDirectories succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  Found {directories.Length} directories:");
            foreach (var dir in directories)
            {
                Console.WriteLine($"    - {dir}");
            }
            Console.ResetColor();
        }

        private static void TestGetFolderUrl(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.GetFolderUrl");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "Level1");
            
            Console.WriteLine($"Getting URL for: {testPath}");
            var startTime = DateTime.Now;
            
            string url = ETCDirectory.GetFolderUrl(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ GetFolderUrl succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  URL: {url}");
            Console.ResetColor();
        }

        private static void TestDeleteNonRecursive(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.Delete (non-recursive)");
            Console.WriteLine("-------------------------------------------");
            
            // Create an empty directory to delete
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "EmptyDirToDelete");
            ETCDirectory.CreateDirectory(testPath, site);
            
            Console.WriteLine($"Deleting empty directory: {testPath}");
            var startTime = DateTime.Now;
            
            ETCDirectory.Delete(testPath, site, recursive: false);
            
            var duration = DateTime.Now - startTime;
            
            // Verify the directory is deleted
            bool stillExists = ETCDirectory.Exists(testPath, site);
            
            if (!stillExists)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Delete (non-recursive) succeeded in {duration.TotalMilliseconds:F2}ms");
                Console.ResetColor();
            }
            else
            {
                throw new Exception("Delete operation failed - directory still exists");
            }
        }

        private static void TestDeleteRecursive(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCDirectory.Delete (recursive)");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "DirectoryTests", "ListingTest");
            
            Console.WriteLine($"Deleting directory recursively: {testPath}");
            var startTime = DateTime.Now;
            
            ETCDirectory.Delete(testPath, site, recursive: true);
            
            var duration = DateTime.Now - startTime;
            
            // Verify the directory is deleted
            bool stillExists = ETCDirectory.Exists(testPath, site);
            
            if (!stillExists)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Delete (recursive) succeeded in {duration.TotalMilliseconds:F2}ms");
                Console.ResetColor();
            }
            else
            {
                throw new Exception("Delete operation failed - directory still exists");
            }
        }
    }
}


