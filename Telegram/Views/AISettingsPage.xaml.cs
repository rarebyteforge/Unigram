using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Telegram.Services;

namespace Telegram.Views
{
    public sealed partial class AISettingsPage : Page
    {
        private AIService _aiService;

        public AISettingsPage()
        {
            this.InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                _aiService = new AIService(null);
                await _aiService.InitializeAsync();

                var isAvailable = await _aiService.IsAIAvailableAsync();
                UpdateAIStatus(isAvailable);

                EnableAIToggle.IsOn = isAvailable;
                OllamaServerInput.Text = "http://localhost:11434";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Settings Initialize Error: {ex.Message}");
            }
        }

        private void UpdateAIStatus(bool isAvailable)
        {
            if (isAvailable)
            {
                AIStatusText.Text = "✓ Ollama is running and ready!";
            }
            else
            {
                AIStatusText.Text = "✗ Ollama is not detected. Please install and start Ollama from https://ollama.ai";
            }
        }

        private async void OnAIToggleChanged(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Debug.WriteLine($"AI Features Toggled: {EnableAIToggle.IsOn}");
        }

        private void OnProviderChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine($"Provider Changed: {ProviderComboBox.SelectedIndex}");
        }
    }
}
