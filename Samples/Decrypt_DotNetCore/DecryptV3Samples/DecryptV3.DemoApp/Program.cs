using DecryptV3.ServiceFactory;
using DecryptV3.UIFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DecryptV3.DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            if (config.GetValue<string>("CERTIFICATE_FILENAME").Trim() == "")
            {
                Console.WriteLine("Certificate FileName Not Found In appsettings.json");
                Console.ReadLine();
                Environment.Exit(0);
            }
            if (config.GetValue<string>("CERTIFICATE_PASSWORD").Trim() == "")
            {
                Console.WriteLine("Certificate Password Not Found In appsettings.json");
                Console.ReadLine();
                Environment.Exit(0);
            }
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IDecryptUIFactory, DecryptUIFactory>();
            services.AddSingleton<IDecryptV3Client, DecryptV3Client>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var uiFactory = serviceProvider.GetService<IDecryptUIFactory>();

            while (true)
            {
                try
                {
                    Console.WriteLine("Please Select an option or service operation");
                    Console.WriteLine("Enter Option number (1: DecryptCardSwipe, 2: DecryptData, 3: GenerateMac)");
                    var keyInfo = Console.ReadKey();
                    Console.WriteLine();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            uiFactory.ShowUI(DecryptUI.DECRYPTCARDSWIPE);
                            break;
                        case ConsoleKey.D2:
                            uiFactory.ShowUI(DecryptUI.DECRYPTDATA);
                            break;
                        case ConsoleKey.D3:
                            uiFactory.ShowUI(DecryptUI.GENERATEMAC);
                            break;
                    }
                    bool decision = Confirm("Would you like to Continue with other Request");
                    if (decision)
                        continue;
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}
