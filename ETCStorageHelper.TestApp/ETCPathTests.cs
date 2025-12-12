using System;

namespace ETCStorageHelper.TestApp
{
    public static class ETCPathTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("         Testing ETCPath Methods");
            Console.WriteLine("==============================================");

            try
            {
                TestCombine();
                TestGetDirectoryName();
                TestGetFileName();
                TestGetExtension();
                TestGetFileNameWithoutExtension();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ All ETCPath tests completed successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ ETCPath tests failed: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private static void TestCombine()
        {
            Console.WriteLine("\n[TEST] ETCPath.Combine");
            Console.WriteLine("-------------------------------------------");

            var tests = new[]
            {
                new { Parts = new[] { "Client A", "2025", "Reports" }, Expected = "Client A/2025/Reports" },
                new { Parts = new[] { "Client A", "2025", "Job001", "report.pdf" }, Expected = "Client A/2025/Job001/report.pdf" },
                new { Parts = new[] { "Client B", "Nested", "Deep", "Path", "File.txt" }, Expected = "Client B/Nested/Deep/Path/File.txt" }
            };

            foreach (var test in tests)
            {
                string result = "";
                if (test.Parts.Length == 2)
                    result = ETCPath.Combine(test.Parts[0], test.Parts[1]);
                else if (test.Parts.Length == 3)
                    result = ETCPath.Combine(test.Parts[0], test.Parts[1], test.Parts[2]);
                else if (test.Parts.Length == 4)
                    result = ETCPath.Combine(test.Parts[0], test.Parts[1], test.Parts[2], test.Parts[3]);
                else if (test.Parts.Length == 5)
                    result = ETCPath.Combine(test.Parts[0], test.Parts[1], test.Parts[2], test.Parts[3], test.Parts[4]);

                if (result == test.Expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ Combine({string.Join(", ", test.Parts)})");
                    Console.WriteLine($"  Result: {result}");
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception($"Combine failed: expected '{test.Expected}', got '{result}'");
                }
            }
        }

        private static void TestGetDirectoryName()
        {
            Console.WriteLine("\n[TEST] ETCPath.GetDirectoryName");
            Console.WriteLine("-------------------------------------------");

            var tests = new[]
            {
                new { Input = "Client A/2025/Reports/report.pdf", Expected = "Client A/2025/Reports" },
                new { Input = "Client A/2025/Reports", Expected = "Client A/2025" },
                new { Input = "Client A", Expected = "" },
                new { Input = "Client A/file.txt", Expected = "Client A" }
            };

            foreach (var test in tests)
            {
                string result = ETCPath.GetDirectoryName(test.Input);

                if (result == test.Expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ GetDirectoryName(\"{test.Input}\")");
                    Console.WriteLine($"  Result: \"{result}\"");
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception($"GetDirectoryName failed: expected '{test.Expected}', got '{result}'");
                }
            }
        }

        private static void TestGetFileName()
        {
            Console.WriteLine("\n[TEST] ETCPath.GetFileName");
            Console.WriteLine("-------------------------------------------");

            var tests = new[]
            {
                new { Input = "Client A/2025/Reports/report.pdf", Expected = "report.pdf" },
                new { Input = "file.txt", Expected = "file.txt" },
                new { Input = "Client A/2025/document.docx", Expected = "document.docx" },
                new { Input = "Client A/2025/Reports/final-report-v2.pdf", Expected = "final-report-v2.pdf" }
            };

            foreach (var test in tests)
            {
                string result = ETCPath.GetFileName(test.Input);

                if (result == test.Expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ GetFileName(\"{test.Input}\")");
                    Console.WriteLine($"  Result: \"{result}\"");
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception($"GetFileName failed: expected '{test.Expected}', got '{result}'");
                }
            }
        }

        private static void TestGetExtension()
        {
            Console.WriteLine("\n[TEST] ETCPath.GetExtension");
            Console.WriteLine("-------------------------------------------");

            var tests = new[]
            {
                new { Input = "report.pdf", Expected = ".pdf" },
                new { Input = "document.docx", Expected = ".docx" },
                new { Input = "Client A/2025/data.xlsx", Expected = ".xlsx" },
                new { Input = "file.tar.gz", Expected = ".gz" },
                new { Input = "noextension", Expected = "" }
            };

            foreach (var test in tests)
            {
                string result = ETCPath.GetExtension(test.Input);

                if (result == test.Expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ GetExtension(\"{test.Input}\")");
                    Console.WriteLine($"  Result: \"{result}\"");
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception($"GetExtension failed: expected '{test.Expected}', got '{result}'");
                }
            }
        }

        private static void TestGetFileNameWithoutExtension()
        {
            Console.WriteLine("\n[TEST] ETCPath.GetFileNameWithoutExtension");
            Console.WriteLine("-------------------------------------------");

            var tests = new[]
            {
                new { Input = "report.pdf", Expected = "report" },
                new { Input = "document.docx", Expected = "document" },
                new { Input = "Client A/2025/final-report-v2.pdf", Expected = "final-report-v2" },
                new { Input = "file.tar.gz", Expected = "file.tar" },
                new { Input = "noextension", Expected = "noextension" }
            };

            foreach (var test in tests)
            {
                string result = ETCPath.GetFileNameWithoutExtension(test.Input);

                if (result == test.Expected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ GetFileNameWithoutExtension(\"{test.Input}\")");
                    Console.WriteLine($"  Result: \"{result}\"");
                    Console.ResetColor();
                }
                else
                {
                    throw new Exception($"GetFileNameWithoutExtension failed: expected '{test.Expected}', got '{result}'");
                }
            }
        }
    }
}


