using System;
using System.Text;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    public static class TestDataGenerator
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Generate random binary data of specified size
        /// </summary>
        public static byte[] GenerateBinaryData(int sizeInBytes)
        {
            byte[] data = new byte[sizeInBytes];
            _random.NextBytes(data);
            return data;
        }

        /// <summary>
        /// Generate random text data of approximately specified length
        /// </summary>
        public static string GenerateTextData(int approximateLength)
        {
            var sb = new StringBuilder();
            string[] words = {
                "Lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
                "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
                "magna", "aliqua", "Ut", "enim", "ad", "minim", "veniam", "quis", "nostrud",
                "exercitation", "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
                "consequat", "Duis", "aute", "irure", "in", "reprehenderit", "voluptate",
                "velit", "esse", "cillum", "fugiat", "nulla", "pariatur"
            };

            while (sb.Length < approximateLength)
            {
                sb.Append(words[_random.Next(words.Length)]);
                sb.Append(" ");
                
                // Add some line breaks
                if (_random.Next(10) == 0)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString().Substring(0, Math.Min(approximateLength, sb.Length));
        }

        /// <summary>
        /// Generate a large file for testing (in MB)
        /// </summary>
        public static byte[] GenerateLargeFile(int sizeInMB)
        {
            Console.WriteLine($"Generating {sizeInMB}MB test file...");
            int sizeInBytes = sizeInMB * 1024 * 1024;
            
            // Generate in chunks to avoid memory issues
            byte[] data = new byte[sizeInBytes];
            int chunkSize = 1024 * 1024; // 1MB chunks
            
            for (int i = 0; i < sizeInBytes; i += chunkSize)
            {
                int remainingSize = Math.Min(chunkSize, sizeInBytes - i);
                byte[] chunk = new byte[remainingSize];
                _random.NextBytes(chunk);
                Array.Copy(chunk, 0, data, i, remainingSize);
                
                if ((i / chunkSize) % 10 == 0)
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
            Console.WriteLine($"✓ Generated {sizeInMB}MB test file");
            
            return data;
        }

        /// <summary>
        /// Cleanup all test data from SharePoint
        /// </summary>
        public static void CleanupTestData(SharePointSite site, string basePath)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("         Cleanup Test Data");
            Console.WriteLine("==============================================");
            Console.WriteLine();
            Console.WriteLine($"This will delete all test data under: {basePath}");
            Console.Write("Are you sure? (yes/no): ");
            
            var response = Console.ReadLine()?.Trim().ToLower();
            
            if (response == "yes")
            {
                try
                {
                    Console.WriteLine($"\nDeleting: {basePath}");
                    
                    if (ETCDirectory.Exists(basePath, site))
                    {
                        ETCDirectory.Delete(basePath, site, recursive: true);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✓ Test data cleaned up successfully!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("⚠ Test data directory does not exist. Nothing to clean up.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"✗ Cleanup failed: {ex.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Cleanup cancelled.");
            }
        }

        /// <summary>
        /// Create a sample PDF-like binary file (not a real PDF, just test data)
        /// </summary>
        public static byte[] GeneratePdfLikeData(int sizeInKB)
        {
            byte[] data = new byte[sizeInKB * 1024];
            
            // Add PDF header-like bytes at the start
            string header = "%PDF-1.4\n";
            byte[] headerBytes = Encoding.ASCII.GetBytes(header);
            Array.Copy(headerBytes, data, headerBytes.Length);
            
            // Fill the rest with random data
            byte[] randomData = new byte[data.Length - headerBytes.Length];
            _random.NextBytes(randomData);
            Array.Copy(randomData, 0, data, headerBytes.Length, randomData.Length);
            
            return data;
        }

        /// <summary>
        /// Format bytes to human-readable size
        /// </summary>
        public static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return $"{len:0.##} {sizes[order]}";
        }
    }
}


