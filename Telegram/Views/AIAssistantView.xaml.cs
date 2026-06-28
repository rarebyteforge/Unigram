using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telegram.Services;
using Windows.UI.Xaml.Input;

namespace Telegram.Views
{
    public sealed partial class AIAssistantView : UserControl
    {
        private AIService _aiService;
        private string _currentText;
        private bool _isProcessing;

        public AIAssistantView()
        {
            this.InitializeComponent();
            InitializeAI();
        }

        private async void InitializeAI()
        {
            try
            {
                _aiService = new AIService(null);
                await _aiService.InitializeAsync();

                var isAvailable = await _aiService.IsAIAvailableAsync();
                UpdateStatus(isAvailable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AI Initialization Error: {ex.Message}");
                UpdateStatus(false);
            }
        }

        private void UpdateStatus(bool isAvailable)
        {
            StatusText.Text = isAvailable ? "✓ AI is ready" : "✗ AI is offline (Ollama not detected)";
            SummarizeButton.IsEnabled = isAvailable;
            RewriteButton.IsEnabled = isAvailable;
            TranslateButton.IsEnabled = isAvailable;
            GenerateButton.IsEnabled = isAvailable;
        }

        private async void OnSummarizeClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ProcessActionAsync("summarize", "Summarizing...");
        }

        private async void OnRewriteClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ProcessActionAsync("rewrite", "Rewriting...");
        }

        private async void OnTranslateClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ProcessActionAsync("translate", "Translating...");
        }

        private async void OnGenerateClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ProcessActionAsync("generate", "Generating response...");
        }

        private async Task ProcessActionAsync(string action, string processingText)
        {
            if (_isProcessing || string.IsNullOrEmpty(_currentText))
                return;

            _isProcessing = true;
            ResponseText.Text = processingText;
            SummarizeButton.IsEnabled = false;
            RewriteButton.IsEnabled = false;
            TranslateButton.IsEnabled = false;
            GenerateButton.IsEnabled = false;

            try
            {
                var result = await _aiService.ProcessMessageAsync(_currentText, action);
                ResponseText.Text = result;
            }
            catch (Exception ex)
            {
                ResponseText.Text = $"Error: {ex.Message}";
            }
            finally
            {
                _isProcessing = false;
                SummarizeButton.IsEnabled = true;
                RewriteButton.IsEnabled = true;
                TranslateButton.IsEnabled = true;
                GenerateButton.IsEnabled = true;
            }
        }

        public void SetInputText(string text)
        {
            _currentText = text;
            ResponseText.Text = "Ready to process text";
        }
    }
}
