using System;
using System.Configuration;
using System.Net;
using ETCStorageHelper;

namespace ETCStorageHelper.TestApp
{
    class Program
    {
        private static SharePointSite _site;
        private static string _basePath;

        static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("   ETC Storage Helper - Test Application");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            // Ensure modern TLS and corporate proxy support before any HTTP calls
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                if (WebRequest.DefaultWebProxy != null)
                {
                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
            }
            catch
            {
                // Non-fatal: continue even if environment does not allow overriding defaults
            }

            // Initialize SharePoint connection
            if (!InitializeSharePointSite())
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Main menu loop
            bool running = true;
            while (running)
            {
                DisplayMenu();
                var choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ETCFileTests.RunAllTests(_site, _basePath);
                            break;
                        case "2":
                            ETCDirectoryTests.RunAllTests(_site, _basePath);
                            break;
                        case "3":
                            ETCPathTests.RunAllTests();
                            break;
                        case "4":
                            ETCFileAsyncTests.RunAllTests(_site, _basePath);
                            break;
                        case "5":
                            IntegrationTests.RunCompleteScenario(_site, _basePath);
                            break;
                        case "6":
                            TestDataGenerator.CleanupTestData(_site, _basePath);
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("Exiting test application...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.WriteLine($"Stack: {ex.StackTrace}");
                    Console.ResetColor();
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static bool InitializeSharePointSite()
        {
            try
            {
                Console.WriteLine("Initializing SharePoint connection...");
                
                var userId = ConfigurationManager.AppSettings["Test.UserId"];
                var userName = ConfigurationManager.AppSettings["Test.UserName"];
                var applicationName = ConfigurationManager.AppSettings["Test.ApplicationName"];
                _basePath = ConfigurationManager.AppSettings["Test.BasePath"];

                _site = SharePointSite.FromConfig(
                    name: "Commercial",
                    configPrefix: "ETCStorage.Commercial",
                    userId: userId,
                    userName: userName,
                    applicationName: applicationName
                );

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Successfully connected to SharePoint!");
                Console.ResetColor();
                Console.WriteLine($"  Site: {_site.SiteUrl}");
                Console.WriteLine($"  Library: {_site.LibraryName}");
                Console.WriteLine($"  User: {userName} ({userId})");
                Console.WriteLine($"  Test Base Path: {_basePath}");
                Console.WriteLine($"  Application Name: {applicationName}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ Failed to initialize SharePoint connection!");
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPlease check your App.config settings:");
                Console.WriteLine("  - ETCStorage.Commercial.TenantId");
                Console.WriteLine("  - ETCStorage.Commercial.ClientId");
                Console.WriteLine("  - ETCStorage.Commercial.ClientSecret");
                Console.WriteLine("  - ETCStorage.Commercial.SiteUrl");
                Console.WriteLine("  - ETCStorage.Commercial.LibraryName");
                return false;
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("                 MAIN MENU");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Test ETCFile Methods");
            Console.WriteLine("2. Test ETCDirectory Methods");
            Console.WriteLine("3. Test ETCPath Methods");
            Console.WriteLine("4. Test ETCFileAsync Methods (Large Files)");
            Console.WriteLine("5. Run Complete Integration Test");
            Console.WriteLine("6. Cleanup Test Data");
            Console.WriteLine("0. Exit");
            Console.WriteLine("==============================================");
            Console.Write("Select an option: ");
        }
    }
}


