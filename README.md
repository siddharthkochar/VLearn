# 🎬 VLearn V2 - AI Video Learning Console App

A minimal console application that converts text input into learning videos using Google Gemini API for script generation and HeyGen API for AI avatar video creation.

## 🎯 Overview

VLearn V2 is a simplified proof-of-concept application that demonstrates an automated pipeline for creating educational videos from text content. The application follows a bare minimum approach with no customizations, no logging, and uses default values for all video generation parameters.

The system uses **HeyGen's AI avatar technology** to create professional-quality educational videos with realistic AI presenters.

## 🚀 Quick Start

### Prerequisites
- .NET 8 Runtime
- Google Gemini API key
- HeyGen API key

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
  "HeyGenApi": {
    "ApiKey": "your-heygen-api-key",
    "BaseUrl": "https://api.heygen.com"
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
2. **🧠 Script Generation** - Converts text to learning script using Google Gemini API
3. **🤖 Video Creation** - Generates video using HeyGen's AI avatar technology
4. **💾 Output** - Downloads MP4 video to local `output/` folder with timestamped naming

## 🎛️ Default Settings

### HeyGen AI Avatar
- **Avatar**: `Abigail_expressive_2024112501` (professional AI presenter)
- **Voice**: `73c0b6a2e29d4d38aca41454bf58c955` (clear English voice)
- **Speed**: 1.1x playback speed for optimal pacing
- **Dimensions**: 1280x720 (HD quality)

### Output Settings
- **Output Format**: MP4 video file
- **File Naming**: `heygen_video_YYYYMMDD_HHMMSS_ScriptTitle.mp4`
- **Processing Time**: 3-5 minutes typical (varies by content length)

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
│   │   ├── HeyGenService.cs    # HeyGen API integration
│   │   └── VideoProcessingService.cs # Video workflow management
│   └── Configuration/          # Configuration models
│       └── AppSettings.cs      # Settings classes for all APIs
├── PROJECT_PLAN.md             # Detailed project plan with all phases
└── README.md                   # This comprehensive documentation
```

## 🛠️ Technology Stack

- **Framework**: .NET 8 Console Application
- **AI Integration**: Google Gemini API (REST)
- **Video Generation**: HeyGen API for AI avatar videos
- **Architecture**: Single provider system with robust error handling
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

### ✅ Phase 3: HeyGen Integration (COMPLETED)
- [x] HeyGen API client with authentication
- [x] AI avatar video creation with default parameters
- [x] Smart video status polling and download
- [x] End-to-end video processing workflow

### ✅ Phase 4: End-to-End Testing & Documentation (COMPLETED)
- [x] Complete pipeline integration testing
- [x] Build verification and configuration validation
- [x] HeyGen video generation workflow testing
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

### HeyGen API (Required)
1. Sign up at [HeyGen](https://app.heygen.com/)
2. Navigate to API settings in your dashboard
3. Generate a new API key
4. Add it to your `appsettings.json` or set `HEYGEN_API_KEY` environment variable

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

🎥 Creating video with HeyGen...
🤖 Submitting HeyGen video creation request...
✅ HeyGen video creation started. Video ID: abc123
⏰ Video is being processed. This typically takes 3-5 minutes...
🔄 Checking HeyGen video status... (Attempt 1/60)
📊 Video status: processing
🔄 Checking HeyGen video status... (Attempt 2/60)
📊 Video status: complete
✅ HeyGen video processing completed!
💾 Downloading HeyGen video...
💾 HeyGen video saved: output/heygen_video_20250713_143022_Learning_Video_Script.mp4
📏 File size: 15.23 MB
🎉 Video ready: output/heygen_video_20250713_143022_Learning_Video_Script.mp4

📋 Generated Script Preview:
==================================================
Hey everyone, and welcome! Today we're diving into the exciting world of Artific
ial Intelligence and Machine Learning – or AI and ML, as they're often called...
==================================================
✅ Process completed successfully!
```

## 🤝 Contributing

This is a proof-of-concept project with a specific minimal scope. Please refer to `PROJECT_PLAN.md` for detailed development guidelines and current status.

**Project Status**: All major phases completed! ✅
- ✅ Phase 1: Console Application Setup
- ✅ Phase 2: Google Gemini Integration  
- ✅ Phase 3: HeyGen Integration
- ✅ Phase 4: Testing & Documentation

Future enhancements could include:
- Additional video providers
- Custom avatar/voice selection
- Batch processing capabilities
- Advanced error recovery and retry logic

## 📄 License

This project is for educational and demonstration purposes.

---

**🎯 Goal**: Simple text-to-video conversion with HeyGen AI avatars  
**⚡ Status**: All Phases Complete - Production Ready ✅  
**📅 Last Updated**: July 13, 2025
