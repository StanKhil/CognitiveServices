using Microsoft.Extensions.Configuration;

namespace CognitiveServices
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var key = config["AzureTranslator:Key"];
            var endpoint = config["AzureTranslator:Endpoint"];
            var region = config["AzureTranslator:Region"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(endpoint))
            {
                Console.WriteLine("Error appsettings.json");
                return;
            }

            var translator = new TranslatorService(endpoint, key, region);

            Console.WriteLine("Azure Translator Demo");

            while (true)
            {
                Console.Write("Enter text | ('exit'): ");
                string input = Console.ReadLine();

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                Console.Write("Enter code ('en', 'uk', 'de', 'cs'): ");
                string targetLang = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(targetLang))
                {
                    Console.WriteLine("Error");
                    continue;
                }

                Console.WriteLine("Translating...");

                string result = await translator.TranslateAsync(input, targetLang);

                Console.WriteLine($"\nTranslated: ({targetLang}): {result}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("Exit");
        }
    }
}
