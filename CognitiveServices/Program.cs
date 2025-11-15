using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

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


            var translatorKey = config["AzureTranslator:Key"];
            var translatorEndpoint = config["AzureTranslator:Endpoint"];
            var translatorRegion = config["AzureTranslator:Region"];

            if (string.IsNullOrEmpty(translatorKey) || string.IsNullOrEmpty(translatorEndpoint))
            {
                Console.WriteLine("Error in AzureTranslator settings in appsettings.json");
                return;
            }

            var translator = new TranslatorService(translatorEndpoint, translatorKey, translatorRegion);

            var ocrKey = config["AzureDocumentIntelligence:Key"];
            var ocrEndpoint = config["AzureDocumentIntelligence:Endpoint"];

            if (string.IsNullOrEmpty(ocrKey) || string.IsNullOrEmpty(ocrEndpoint))
            {
                Console.WriteLine("Error in AzureDocumentIntelligence settings in appsettings.json");
                return;
            }

            var ocrService = new OcrService(ocrEndpoint, ocrKey);

            Console.WriteLine("OCR + Translator Demo");

            while (true)
            {
                //Console.Write("Enter image path | ('exit'): ");
                //string imagePath = Console.ReadLine();

                //if (string.Equals(imagePath, "exit", StringComparison.OrdinalIgnoreCase))
                //    break;

                string imagePath = "C:\\Users\\user\\OneDrive\\Рабочий стол\\Screenshot_1.png";

                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("File not found.");
                    continue;
                }

                Console.WriteLine("Extracting text from image...");
                string extractedText = await ocrService.ExtractTextAsync(imagePath);
                Console.WriteLine($"\nExtracted text:\n{extractedText}");
                Console.WriteLine(new string('-', 40));

                Console.Write("Enter target language code ('en', 'uk', 'de', 'cs'): ");
                string targetLang = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(targetLang))
                {
                    Console.WriteLine("Invalid language code.");
                    continue;
                }

                Console.WriteLine("Translating...");
                string translatedText = await translator.TranslateAsync(extractedText, targetLang);

                Console.WriteLine($"\nTranslated ({targetLang}):\n{translatedText}");
                Console.WriteLine(new string('-', 40));
            }

            Console.WriteLine("Exit");
        }
    }
}
