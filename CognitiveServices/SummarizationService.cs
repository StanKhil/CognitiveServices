using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServices
{
    internal class SummarizationService
    {
        private readonly TextAnalyticsClient _client;

        public SummarizationService(string endpoint, string key)
        {
            _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        public async Task<string> SummarizeAsync(string text)
        {
            // ОБОВ’ЯЗКОВО — TextDocumentInput
            var documents = new List<TextDocumentInput>
            {
                new TextDocumentInput("1", text)
                {
                    Language = "en"    // або "uk" / "ru" / "cs"
                }
            };

            var options = new ExtractiveSummarizeOptions()
            {
                MaxSentenceCount = 5
            };

            // ЄДИНА перегрузка, доступна у твоєму SDK
            ExtractiveSummarizeOperation operation =
                await _client.ExtractiveSummarizeAsync(
                    WaitUntil.Completed,
                    documents,
                    options
                );

            //ExtractiveSummarizeResult result = operation.Value[0];

            //var sentences = result.Sentences.Select(s => s.Text);

            //return string.Join(" ", sentences);
            throw new NotImplementedException("Summarization logic to be implemented based on SDK capabilities.");
        }
    }
}
