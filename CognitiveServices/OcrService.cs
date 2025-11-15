using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace CognitiveServices
{
    internal class OcrService
    {
        private readonly DocumentAnalysisClient _client;

        public OcrService(string endpoint, string key)
        {
            _client = new DocumentAnalysisClient(
                new Uri(endpoint),
                new AzureKeyCredential(key)
            );
        }

        public async Task<string> ExtractTextAsync(string imagePath)
        {
            using FileStream stream = File.OpenRead(imagePath);

            AnalyzeDocumentOperation operation =
                await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", stream);

            AnalyzeResult result = operation.Value;

            var textLines = result.Pages
                .SelectMany(p => p.Lines)
                .Select(line => line.Content);

            return string.Join("\n", textLines);
        }
    }
}
