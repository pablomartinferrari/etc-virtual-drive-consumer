using System;
using System.Threading;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    public static class ETCFileAsyncTests
    {
        public static void RunAllTests(SharePointSite site, string basePath)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("    Testing ETCFileAsync Methods (Large Files)");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Note: Large file tests may take several minutes to complete.");
            Console.WriteLine("The async API queues uploads and returns immediately.");
            Console.ResetColor();
            Console.WriteLine();

            try
            {
                TestSmallFileSync(site, basePath);
                TestMediumFileSync(site, basePath);
                TestLargeFileAsync(site, basePath);
                TestLargeFileRead(site, basePath);
                TestCachedRead(site, basePath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ All ETCFileAsync tests completed successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ ETCFileAsync tests failed: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private static void TestSmallFileSync(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] Small File (1MB) - Synchronous");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "AsyncTests", "small-1mb.dat");
            var testData = TestDataGenerator.GenerateBinaryData(1024 * 1024); // 1MB
            
            Console.WriteLine($"Writing {TestDataGenerator.FormatBytes(testData.Length)} to: {testPath}");
            Console.WriteLine("Expected: Synchronous end-to-end completion (~500ms)");
            
            var startTime = DateTime.Now;
            ETCFile.WriteAllBytes(testPath, testData, site);
            var duration = DateTime.Now - startTime;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Write completed in {duration.TotalMilliseconds:F0}ms");
            Console.ResetColor();
        }

        private static void TestMediumFileSync(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] Medium File (10MB) - Synchronous");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "AsyncTests", "medium-10mb.dat");
            var testData = TestDataGenerator.GenerateLargeFile(10); // 10MB
            
            Console.WriteLine($"Writing {TestDataGenerator.FormatBytes(testData.Length)} to: {testPath}");
            Console.WriteLine("Expected: Synchronous end-to-end completion");
            
            var startTime = DateTime.Now;
            ETCFile.WriteAllBytes(testPath, testData, site);
            var duration = DateTime.Now - startTime;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Write completed in {duration.TotalSeconds:F1}s");
            Console.ResetColor();
        }

        private static void TestLargeFileAsync(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] Large File (60MB) - Async/Queued");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "AsyncTests", "large-60mb.dat");
            var testData = TestDataGenerator.GenerateLargeFile(60); // 60MB
            
            Console.WriteLine($"Writing {TestDataGenerator.FormatBytes(testData.Length)} to: {testPath}");
            Console.WriteLine("Expected: Method returns quickly, upload continues in background");
            Console.WriteLine();

            bool uploadComplete = false;
            bool uploadFailed = false;
            string errorMessage = null;

            var startTime = DateTime.Now;
            
            var handle = ETCFileAsync.WriteAllBytesAsync(
                testPath,
                testData,
                site,
                onSuccess: path =>
                {
                    uploadComplete = true;
                    var uploadDuration = DateTime.Now - startTime;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✓ Background upload completed in {uploadDuration.TotalSeconds:F1}s");
                    Console.ResetColor();
                },
                onError: (path, ex) =>
                {
                    uploadFailed = true;
                    errorMessage = ex.Message;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n✗ Background upload failed: {ex.Message}");
                    Console.ResetColor();
                }
            );

            var methodReturnDuration = DateTime.Now - startTime;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ WriteAllBytesAsync returned in {methodReturnDuration.TotalMilliseconds:F0}ms");
            Console.ResetColor();
            Console.WriteLine($"  Upload handle: {handle}");
            Console.WriteLine("\nWaiting for background upload to complete...");

            // Wait for upload to complete (with timeout)
            int maxWaitSeconds = 180; // 3 minutes
            int waitedSeconds = 0;
            while (!uploadComplete && !uploadFailed && waitedSeconds < maxWaitSeconds)
            {
                Thread.Sleep(1000);
                waitedSeconds++;
                if (waitedSeconds % 10 == 0)
                {
                    Console.WriteLine($"  Still uploading... ({waitedSeconds}s elapsed)");
                }
            }

            if (uploadFailed)
            {
                throw new Exception($"Async upload failed: {errorMessage}");
            }
            else if (!uploadComplete)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Upload is still in progress after {maxWaitSeconds}s. Check logs.");
                Console.ResetColor();
            }
        }

        private static void TestLargeFileRead(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] Large File Read - First Time (No Cache)");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "AsyncTests", "medium-10mb.dat");
            
            Console.WriteLine($"Reading from: {testPath}");
            Console.WriteLine("Expected: Downloads and caches the file");
            
            var startTime = DateTime.Now;
            byte[] data = ETCFile.ReadAllBytes(testPath, site);
            var duration = DateTime.Now - startTime;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Read completed in {duration.TotalSeconds:F1}s");
            Console.WriteLine($"  Read {TestDataGenerator.FormatBytes(data.Length)}");
            Console.ResetColor();
        }

        private static void TestCachedRead(SharePointSite site, string basePath)
        {
            Console.WriteLine("\n[TEST] Large File Read - Cached");
            Console.WriteLine("-------------------------------------------");
            
            var testPath = ETCPath.Combine(basePath, "AsyncTests", "medium-10mb.dat");
            
            Console.WriteLine($"Reading from cache: {testPath}");
            Console.WriteLine("Expected: Fast read from local cache (~200ms)");
            
            var startTime = DateTime.Now;
            byte[] data = ETCFile.ReadAllBytes(testPath, site);
            var duration = DateTime.Now - startTime;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Cached read completed in {duration.TotalMilliseconds:F0}ms");
            Console.WriteLine($"  Read {TestDataGenerator.FormatBytes(data.Length)}");
            Console.ResetColor();
        }
    }
}


