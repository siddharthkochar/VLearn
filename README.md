# 🎬 VLearn V2 - AI Video Learning Console App

A minimal console application that converts text input into learning videos using Google Gemini API for script generation and dual video providers (DeepBrainAI primary, Synthesia fallback) for video creation.

## 🎯 Overview

VLearn V2 is a simplified proof-of-concept application that demonstrates an automated pipeline for creating educational videos from text content. The application follows a bare minimum approach with no customizations, no logging, and uses default values for all video generation parameters.

The system supports **dual video providers** with DeepBrainAI as the primary provider and Synthesia as the fallback provider, ensuring high availability and redundancy.

## 🚀 Quick Start

### Prerequisites
- .NET 8 Runtime
- Google Gemini API key
- Synthesia API key
- DeepBrainAI API key (for Phase 4 - optional)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/siddharthkochar/VLearn.git
cd VLearn
```

2. Configure API keys in `VLearn.Console/appsettings.json`:
```json
{
  "GeminiApi": {
    "ApiKey": "your-gemini-api-key",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta"
  },
  "SynthesiaApi": {
    "ApiKey": "your-synthesia-api-key",
    "BaseUrl": "https://api.synthesia.io/v2"
  },
  "DeepBrainApi": {
    "ApiKey": "your-deepbrain-api-key",
    "BaseUrl": "https://v2.aistudios.com/api/odin"
  }
}
```

3. Build the application:
```bash
cd VLearn.Console
dotnet build
```

### Usage

#### File Input Mode
```bash
dotnet run -- generate input.txt
```

#### Interactive Mode
```bash
dotnet run -- generate
```
Then enter your text content and press Enter twice to finish.

## 🔄 How It Works

1. **📖 Input Processing** - Reads text from file or console input
2. **🧠 Script Generation** - Converts text to learning script using Google Gemini API *(Phase 2 - COMPLETED)*
3. **🤖 Primary Video Creation** - Generates video using DeepBrainAI API with default AI model *(Phase 4 - COMPLETED)*
4. **🔄 Fallback Processing** - If DeepBrainAI fails, automatically switches to Synthesia API *(Phase 3 - COMPLETED)*
5. **💾 Output** - Downloads MP4 video to local `output/` folder with provider-specific naming

## 🎛️ Default Settings

### DeepBrainAI (Primary Provider)
- **AI Model**: `ysy` (default AI model)
- **Clothes**: Default configuration (`"1"`)
- **Language**: Auto-detected (`"en"` for English)
- **Voice**: Default DeepBrainAI voice for selected model

### Synthesia (Fallback Provider)
- **Avatar**: `anna_costume1_cameraA`
- **Background**: `green_screen`
- **Voice**: Default Synthesia voice for selected avatar

### Output Settings
- **Output Format**: MP4 video file
- **Primary Output**: `output/deepbrain_video_YYYYMMDD_HHMMSS_title.mp4`
- **Fallback Output**: `output/synthesia_video_YYYYMMDD_HHMMSS_title.mp4`
- **Processing Time**: 3-5 minutes typical (varies by provider and content length)

## 📁 Project Structure

```
VLearn/
├── VLearn.Console/               # Main console application
│   ├── Program.cs               # Application entry point with dual provider DI
│   ├── appsettings.json        # Configuration file with all API settings
│   ├── Models/                 # Data models
│   │   ├── InputText.cs        # Input handling model
│   │   ├── Script.cs           # Script model with metadata
│   │   ├── VideoRequest.cs     # Video request model
│   │   └── ApiModels.cs        # API response models for all providers
│   ├── Services/               # Application services
│   │   ├── InputService.cs     # Input processing service
│   │   ├── GeminiService.cs    # Gemini API integration
│   │   ├── SynthesiaService.cs # Synthesia API integration
│   │   ├── DeepBrainService.cs # DeepBrainAI API integration
│   │   ├── VideoProcessingService.cs # Original Synthesia workflow
│   │   └── DualVideoProcessingService.cs # Dual provider management
│   └── Configuration/          # Configuration models
│       └── AppSettings.cs      # Settings classes for all APIs
├── PROJECT_PLAN.md             # Detailed project plan with all phases
└── README.md                   # This comprehensive documentation
```

## 🛠️ Technology Stack

- **Framework**: .NET 8 Console Application
- **AI Integration**: Google Gemini API (REST)
- **Video Generation**: DeepBrainAI API (primary), Synthesia API (fallback)
- **Architecture**: Dual provider system with automatic failover
- **Dependencies**: Minimal - HTTP client, JSON serialization, configuration management

## 📊 Development Status

### ✅ Phase 1: Basic Console Application (COMPLETED)
- [x] Console application with dependency injection
- [x] File and interactive input handling
- [x] Basic configuration system
- [x] Core data models

### ✅ Phase 2: Google Gemini Integration (COMPLETED)
- [x] Gemini API client implementation
- [x] Script generation from text input
- [x] Prompt template for educational content
- [x] Intelligent script parsing and validation

### ✅ Phase 3: Synthesia Integration (COMPLETED)
- [x] Synthesia API client with authentication
- [x] Video creation with default parameters
- [x] Smart video status polling and download
- [x] End-to-end video processing workflow

### ✅ Phase 4: DeepBrainAI Integration (COMPLETED)
- [x] DeepBrainAI API client implementation
- [x] AI model integration with default settings
- [x] Dual provider support system
- [x] Enhanced file naming with provider prefixes
- [x] Automatic failover mechanism

### ✅ Phase 5: End-to-End Testing & Documentation (COMPLETED)
- [x] Complete pipeline integration testing
- [x] Build verification and configuration validation
- [x] Dual provider workflow testing
- [x] Comprehensive usage documentation

## 🚫 Limitations

By design, this application:
- ❌ Has no logging or debug output
- ❌ Offers no video customization options
- ❌ Supports only text input (no PDFs, URLs, etc.)
- ❌ Processes one video at a time
- ❌ Has minimal error recovery
- ❌ Uses only default Synthesia settings

## 🔑 API Key Setup

### Google Gemini API
1. Visit [Google AI Studio](https://makersuite.google.com/app/apikey)
2. Create a new API key
3. Add it to your `appsettings.json` or set `GEMINI_API_KEY` environment variable

### DeepBrainAI API (Required for Primary Provider)
1. Sign up at [DeepBrain AI Studios](https://www.aistudios.com/)
2. Navigate to API settings in your dashboard
3. Generate a new API key
4. Add it to your `appsettings.json` or set `DEEPBRAIN_API_KEY` environment variable

### Synthesia API (Required for Fallback Provider)
1. Sign up at [Synthesia](https://www.synthesia.io/)
2. Go to Account → Integrations
3. Create a new API key
4. Add it to your `appsettings.json` or set `SYNTHESIA_API_KEY` environment variable

## 📝 Example Usage

### Sample Input File (sample-input.txt)
```text
Artificial Intelligence and Machine Learning

AI and ML are transformative technologies that are reshaping how we solve complex problems. Machine learning allows computers to learn patterns from data without being explicitly programmed for each specific task.

Key concepts include:
- Supervised learning: Training with labeled data
- Unsupervised learning: Finding patterns in unlabeled data  
- Neural networks: Models inspired by the human brain
- Deep learning: Neural networks with multiple layers

These technologies are being applied across industries from healthcare to finance, enabling automation, prediction, and decision-making at unprecedented scale.
```

### Running the Application
```bash
cd VLearn.Console
dotnet run -- generate sample-input.txt
```

### Expected Output
```
🎬 VLearn V2 - AI Video Learning Console App
==================================================
📖 Processing input...
✅ Input received from: sample-input.txt
📝 Content length: 673 characters

🧠 Generating script with Gemini...
🔗 Calling Gemini API...
✅ Script generated successfully!
📝 Script title: Learning Video Script
⏱️ Estimated duration: 128 seconds

🎥 Creating video with AI providers...
🎬 Attempting video creation with DeepBrainAI...
🤖 Submitting DeepBrainAI video creation request...
✅ DeepBrainAI video creation started. Project Key: abc123
⏰ Video is being processed. This typically takes 3-5 minutes...
🔄 Checking DeepBrainAI project status... (Attempt 1/60)
📊 Project status: in_progress
🔄 Checking DeepBrainAI project status... (Attempt 2/60)
📊 Project status: complete
✅ DeepBrainAI video processing completed!
💾 Downloading DeepBrainAI video...
💾 DeepBrainAI video saved: output/deepbrain_video_20250710_143022_Learning_Video_Script.mp4
📏 File size: 15.23 MB
🎉 Video ready: output/deepbrain_video_20250710_143022_Learning_Video_Script.mp4

📋 Generated Script Preview:
==================================================
Hey everyone, and welcome! Today we're diving into the exciting world of Artific
ial Intelligence and Machine Learning – or AI and ML, as they're often called...
==================================================
✅ Process completed successfully!
```

## 🔄 Dual Provider System

VLearn V2 features an intelligent dual provider system that ensures high availability:

### Provider Priority
1. **Primary**: DeepBrainAI API (faster processing, AI-powered avatars)
2. **Fallback**: Synthesia API (reliable backup, traditional avatars)

### Automatic Failover
- If DeepBrainAI fails or times out, automatically switches to Synthesia
- Preserves all processing and continues seamlessly
- Different file naming to identify which provider was used

### Provider-Specific Features
- **DeepBrainAI**: AI model `ysy`, clothes config `"1"`, language auto-detection
- **Synthesia**: Avatar `anna_costume1_cameraA`, `green_screen` background

### Troubleshooting
- **Both providers fail**: Check API keys and network connectivity
- **DeepBrainAI only fails**: Will automatically use Synthesia (normal operation)
- **Long processing times**: Both providers typically take 3-5 minutes
- **Configuration errors**: Verify `appsettings.json` is in output directory

## 🤝 Contributing

This is a proof-of-concept project with a specific minimal scope. Please refer to `PROJECT_PLAN.md` for detailed development guidelines and current status.

**Project Status**: All major phases completed! ✅
- ✅ Phase 1: Console Application Setup
- ✅ Phase 2: Google Gemini Integration  
- ✅ Phase 3: Synthesia Integration
- ✅ Phase 4: DeepBrainAI Integration
- ✅ Phase 5: Testing & Documentation

Future enhancements could include:
- Additional video providers
- Custom avatar/model selection
- Batch processing capabilities
- Advanced error recovery and retry logic

## 📄 License

This project is for educational and demonstration purposes.

---

**🎯 Goal**: Simple text-to-video conversion with dual provider reliability  
**⚡ Status**: All Phases Complete - Production Ready ✅  
**📅 Last Updated**: July 10, 2025
