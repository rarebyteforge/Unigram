using Microsoft.UI.Xaml.Controls;
using System;
using Telegram.Services;

namespace Telegram.Views
{
    public sealed partial class AIAssistantPanel : UserControl
    {
        private AIService _aiService;
        public event Action<string, string> OnActionRequested;

        public AIAssistantPanel()
        {
            this.InitializeComponent();
            InitializeAI();
        }

        private async void InitializeAI()
        {
            _aiService = new AIService(null);
            await _aiService.InitializeAsync();
        }

        private void OnSummarizeClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            OnActionRequested?.Invoke("summarize", "Summarizing...");
        }

        private void OnRewriteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            OnActionRequested?.Invoke("rewrite", "Rewriting...");
        }

        private void OnTranslateClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            OnActionRequested?.Invoke("translate", "Translating...");
        }
    }
}
