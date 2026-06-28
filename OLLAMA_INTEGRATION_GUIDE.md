# Ollama AI Integration Guide for Unigram

## Overview
This guide explains how to use the new Ollama AI integration in Unigram. Ollama allows you to run large language models locally on your computer with no API keys required.

## Installation

### 1. Install Ollama
- Download from: https://ollama.ai
- Run the installer and follow the setup wizard
- Ollama will start automatically and run on `http://localhost:11434`

### 2. Download a Model
After installing Ollama, open Command Prompt and run:
```bash
ollama pull mistral
```

Other recommended models:
```bash
ollama pull llama2
ollama pull neural-chat
ollama pull dolphin-mixtral
```

### 3. Start Ollama
Ollama runs in the background by default. To verify it's running:
```bash
curl http://localhost:11434/api/tags
```

## Features

### AI Assistant View
- **Summarize**: Create a concise summary of selected text
- **Rewrite**: Improve clarity and conciseness of text
- **Translate**: Translate text to different languages
- **Generate**: Ask AI any question or get creative content

### Integration Points
1. **Chat Messages**: Quick action buttons on right-click menu
2. **Settings Page**: Configure AI preferences and model selection
3. **AI Assistant Panel**: Dedicated UI for AI interactions

## Usage

### In Chat
1. Right-click on a message
2. Select AI action (Summarize, Rewrite, Translate)
3. View the AI-generated response

### AI Assistant View
1. Open the AI Assistant panel (from Settings → AI Assistant)
2. Paste or type text
3. Click an action button
4. View the response

### Settings
1. Go to Settings → AI Assistant
2. Configure AI provider and model
3. Enable/disable features as desired
4. Save preferences

## Architecture

### Service Layer (`OllamaService.cs`)
- `IAIProvider` - Interface for AI providers
- `OllamaProvider` - Local Ollama implementation
- `AIService` - Main service class

### UI Components
- `AIAssistantView` - Full AI interface
- `AIAssistantPanel` - Quick action panel
- `AISettingsPage` - Settings and configuration

## Privacy & Performance

- ✓ **100% Local**: No data sent to cloud services
- ✓ **Offline**: Works without internet connection
- ✓ **Fast**: Response times typically 2-10 seconds
- ✓ **Free**: No API keys or subscriptions needed

## Troubleshooting

### Ollama Not Detected
1. Verify Ollama is installed: https://ollama.ai
2. Check it's running: `curl http://localhost:11434/api/tags`
3. Restart Unigram

### Slow Responses
- Ensure you have enough system RAM (8GB+ recommended)
- Larger models require more memory
- Close other memory-intensive applications

### Model Download Issues
```bash
# Clear cache if download fails
ollama rm mistral
ollama pull mistral
```

## Future Enhancements

- [ ] Support for additional AI providers (Faraday, HuggingFace)
- [ ] Model fine-tuning for better results
- [ ] Custom prompts library
- [ ] Response caching to improve performance
- [ ] Voice input support

## Support
For issues or feature requests, visit: https://github.com/rarebyteforge/Unigram
