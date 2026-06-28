using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Td.Api;

namespace Telegram.Services
{
    public interface IAIProvider
    {
        Task<string> GenerateResponseAsync(string prompt);
        Task<string> SummarizeTextAsync(string text);
        Task<string> TranslateTextAsync(string text, string targetLanguage = "en");
        Task<string> RewriteTextAsync(string text);
        Task<bool> IsAvailableAsync();
    }

    public class OllamaProvider : IAIProvider
    {
        private readonly HttpClient _httpClient;
        private const string OllamaBaseUrl = "http://localhost:11434";
        private string _selectedModel = "mistral";

        public OllamaProvider()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{OllamaBaseUrl}/api/tags", HttpCompletionOption.ResponseHeadersRead);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            try
            {
                var request = new
                {
                    model = _selectedModel,
                    prompt = prompt,
                    stream = false
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{OllamaBaseUrl}/api/generate", jsonContent);
                if (!response.IsSuccessStatusCode)
                    return "Error: Could not generate response";

                var responseContent = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("response", out var responseText))
                    {
                        return responseText.GetString() ?? "No response generated";
                    }
                }

                return "Error: Invalid response format";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ollama GenerateResponse Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SummarizeTextAsync(string text)
        {
            var prompt = $"Summarize the following text concisely:\n\n{text}\n\nSummary:";
            return await GenerateResponseAsync(prompt);
        }

        public async Task<string> TranslateTextAsync(string text, string targetLanguage = "en")
        {
            var prompt = $"Translate the following text to {targetLanguage}:\n\n{text}\n\nTranslation:";
            return await GenerateResponseAsync(prompt);
        }

        public async Task<string> RewriteTextAsync(string text)
        {
            var prompt = $"Rewrite the following text in a clearer and more concise way:\n\n{text}\n\nRewritten:";
            return await GenerateResponseAsync(prompt);
        }
    }

    public class AIService : ServiceBase
    {
        private IAIProvider _provider;
        private bool _isInitialized;

        public AIService(IClientService clientService) : base(clientService)
        {
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _provider = new OllamaProvider();
            
            if (await _provider.IsAvailableAsync())
            {
                Debug.WriteLine("✓ Ollama AI Provider initialized successfully");
            }
            else
            {
                Debug.WriteLine("✗ Ollama not available. AI features will be limited.");
            }

            _isInitialized = true;
        }

        public async Task<string> ProcessMessageAsync(string message, string action)
        {
            if (_provider == null)
                await InitializeAsync();

            return action switch
            {
                "summarize" => await _provider.SummarizeTextAsync(message),
                "rewrite" => await _provider.RewriteTextAsync(message),
                "translate" => await _provider.TranslateTextAsync(message),
                _ => await _provider.GenerateResponseAsync(message)
            };
        }

        public async Task<bool> IsAIAvailableAsync()
        {
            if (_provider == null)
                await InitializeAsync();

            return _provider != null && await _provider.IsAvailableAsync();
        }
    }
}
