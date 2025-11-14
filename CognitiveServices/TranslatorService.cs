using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CognitiveServices
{
    public class TranslatorService
    {
        private readonly string _endpoint;
        private readonly string _key;
        private readonly string _region;

        public TranslatorService(string endpoint, string key, string region = "")
        {
            _endpoint = endpoint;
            _key = key;
            _region = region;
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage)
        {
            using var http = new HttpClient();

            string route = $"/translate?api-version=3.0&to={targetLanguage}";
            var requestUri = new Uri(new Uri(_endpoint), route);

            var requestBody = new[] { new { Text = text } };
            var jsonBody = JsonSerializer.Serialize(requestBody);

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _key);

            if (!string.IsNullOrWhiteSpace(_region))
            {
                request.Headers.Add("Ocp-Apim-Subscription-Region", _region);
            }

            var response = await http.SendAsync(request);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error: {response.StatusCode}\n{jsonResponse}";
            }

            using var doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            try
            {
                var translation = root[0].GetProperty("translations")[0].GetProperty("text").GetString();
                return translation ?? "(null)";
            }
            catch
            {
                return "Error:\n" + jsonResponse;
            }
        }
    }
}
