using System;
using System.Text;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    public static class ETCFileTests
    {
        public static void RunAllTests(SharePointSite site, string basePath)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("         Testing ETCFile Methods");
            Console.WriteLine("==============================================");

            try
            {
                TestWriteAllBytes(site, basePath);
                TestWriteAllText(site, basePath);
                TestReadAllBytes(site, basePath);
                TestReadAllText(site, basePath);
                TestExists(site, basePath);
                TestCopy(site, basePath);
                TestGetFileUrl(site, basePath);
                TestDelete(site, basePath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ All ETCFile tests completed successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ ETCFile tests failed: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private static void TestWriteAllBytes(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.WriteAllBytes");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-binary.dat");
            var testData = TestDataGenerator.GenerateBinaryData(1024); // 1KB
            
            Console.WriteLine($"Writing {testData.Length} bytes to: {testPath}");
            var startTime = DateTime.Now;
            
            ETCFile.WriteAllBytes(testPath, testData, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ WriteAllBytes succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.ResetColor();
        }

        private static void TestWriteAllText(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.WriteAllText");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-text.txt");
            var testText = TestDataGenerator.GenerateTextData(500);
            
            Console.WriteLine($"Writing {testText.Length} characters to: {testPath}");
            var startTime = DateTime.Now;
            
            ETCFile.WriteAllText(testPath, testText, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ WriteAllText succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.ResetColor();
        }

        private static void TestReadAllBytes(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.ReadAllBytes");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-binary.dat");
            
            Console.WriteLine($"Reading bytes from: {testPath}");
            var startTime = DateTime.Now;
            
            byte[] data = ETCFile.ReadAllBytes(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ ReadAllBytes succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  Read {data.Length} bytes");
            Console.ResetColor();
        }

        private static void TestReadAllText(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.ReadAllText");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-text.txt");
            
            Console.WriteLine($"Reading text from: {testPath}");
            var startTime = DateTime.Now;
            
            string text = ETCFile.ReadAllText(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ ReadAllText succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  Read {text.Length} characters");
            Console.WriteLine($"  Preview: {text.Substring(0, Math.Min(50, text.Length))}...");
            Console.ResetColor();
        }

        private static void TestExists(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.Exists");
            Console.WriteLine("-------------------------------------------");
            
            var existingPath = ETCPath.Combine(basePath, "FileTests", "test-text.txt");
            var nonExistingPath = ETCPath.Combine(basePath, "FileTests", "does-not-exist.txt");
            
            Console.WriteLine($"Checking if file exists: {existingPath}");
            bool exists1 = ETCFile.Exists(existingPath, site);
            
            Console.WriteLine($"Checking if file exists: {nonExistingPath}");
            bool exists2 = ETCFile.Exists(nonExistingPath, site);
            
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

        private static void TestCopy(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.Copy");
            Console.WriteLine("-------------------------------------------");
            
            var sourcePath = ETCPath.Combine(basePath, "FileTests", "test-text.txt");
            var destPath = ETCPath.Combine(basePath, "FileTests", "test-text-copy.txt");
            
            Console.WriteLine($"Copying file from: {sourcePath}");
            Console.WriteLine($"               to: {destPath}");
            var startTime = DateTime.Now;
            
            ETCFile.Copy(sourcePath, destPath, site);
            
            var duration = DateTime.Now - startTime;
            
            // Verify the copy exists
            bool copyExists = ETCFile.Exists(destPath, site);
            
            if (copyExists)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Copy succeeded in {duration.TotalMilliseconds:F2}ms");
                Console.ResetColor();
            }
            else
            {
                throw new Exception("Copy operation failed - destination file does not exist");
            }
        }

        private static void TestGetFileUrl(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.GetFileUrl");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-text.txt");
            
            Console.WriteLine($"Getting URL for: {testPath}");
            var startTime = DateTime.Now;
            
            string url = ETCFile.GetFileUrl(testPath, site);
            
            var duration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ GetFileUrl succeeded in {duration.TotalMilliseconds:F2}ms");
            Console.WriteLine($"  URL: {url}");
            Console.ResetColor();
        }

        private static void TestDelete(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] ETCFile.Delete");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "FileTests", "test-text-copy.txt");
            
            Console.WriteLine($"Deleting file: {testPath}");
            var startTime = DateTime.Now;
            
            ETCFile.Delete(testPath, site);
            
            var duration = DateTime.Now - startTime;
            
            // Verify the file is deleted
            bool stillExists = ETCFile.Exists(testPath, site);
            
            if (!stillExists)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Delete succeeded in {duration.TotalMilliseconds:F2}ms");
                Console.ResetColor();
            }
            else
            {
                throw new Exception("Delete operation failed - file still exists");
            }
        }
    }
}


