# 🎬 VLearn V2 - AI Video Learning Console App

A minimal console application that converts text input into learning videos using Google Gemini API for script generation and multiple video providers (Synthesia and DeepBrainAI) for video creation.

## 🎯 Overview

VLearn V2 is a simplified proof-of-concept application that demonstrates an automated pipeline for creating educational videos from text content. The application follows a bare minimum approach with no customizations, no logging, and uses default values for all video generation parameters.

The system currently supports Synthesia for video generation, with DeepBrainAI integration planned for additional video provider options.

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
3. **🎬 Video Creation** - Generates video using Synthesia API with default settings *(Phase 3 - COMPLETED)*
4. **🤖 AI Video Generation** - Alternative video generation using DeepBrainAI API *(Phase 4 - PLANNED)*
5. **💾 Output** - Downloads MP4 video to local `output/` folder

## 🎛️ Default Settings

### Synthesia (Current Provider)
- **Avatar**: `anna_costume1_cameraA`
- **Background**: `green_screen`
- **Voice**: Default Synthesia voice for selected avatar

### DeepBrainAI (Planned Provider)
- **AI Model**: `ysy` or default model
- **Clothes**: Default configuration (`"1"`)
- **Language**: Auto-detected (`"en"` for English)

### Output Settings
- **Output Format**: MP4 video file
- **Output Location**: `output/synthesia_video_YYYYMMDD_HHMMSS.mp4` or `output/deepbrain_video_YYYYMMDD_HHMMSS.mp4`

## 📁 Project Structure

```
VLearn/
├── VLearn.Console/               # Main console application
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json        # Configuration file
│   ├── Models/                 # Data models
│   │   ├── InputText.cs        # Input handling model
│   │   ├── Script.cs           # Script model
│   │   ├── VideoRequest.cs     # Video request model
│   │   └── ApiModels.cs        # API response models
│   ├── Services/               # Application services
│   │   ├── InputService.cs     # Input processing service
│   │   ├── GeminiService.cs    # Gemini API integration
│   │   ├── SynthesiaService.cs # Synthesia API integration
│   │   └── VideoProcessingService.cs # Video workflow management
│   └── Configuration/          # Configuration models
│       └── AppSettings.cs      # Settings classes
├── PROJECT_PLAN.md             # Updated project plan with phases
└── README.md                   # This file
```

## 🛠️ Technology Stack

- **Framework**: .NET 8 Console Application
- **AI Integration**: Google Gemini API (REST)
- **Video Generation**: Synthesia API (primary), DeepBrainAI API (planned)
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

### � Phase 4: DeepBrainAI Integration (IN DEVELOPMENT)
- [ ] DeepBrainAI API client implementation
- [ ] AI model integration with default settings
- [ ] Dual provider support system
- [ ] Enhanced file naming with provider prefixes

### �📋 Phase 5: End-to-End Testing (PLANNED)
- [ ] Complete pipeline integration testing
- [ ] Error handling and user feedback improvements
- [ ] Documentation completion

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

### Synthesia API
1. Sign up at [Synthesia](https://www.synthesia.io/)
2. Go to Account → Integrations
3. Create a new API key
4. Add it to your `appsettings.json` or set `SYNTHESIA_API_KEY` environment variable

### DeepBrainAI API (Optional - Phase 4)
1. Sign up at [DeepBrain AI Studios](https://www.aistudios.com/)
2. Navigate to API settings in your dashboard
3. Generate a new API key
4. Add it to your `appsettings.json` or set `DEEPBRAIN_API_KEY` environment variable

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

� Creating video with Synthesia...
🎬 Submitting video creation request...
✅ Video creation started. Video ID: abc123
⏰ Video is being processed. This typically takes 3-5 minutes...
🔄 Checking video status... (Attempt 1/60)
📊 Video status: in_progress
🔄 Checking video status... (Attempt 2/60)
📊 Video status: complete
✅ Video processing completed!
💾 Downloading video...
💾 Video saved: output/synthesia_video_20250708_143022.mp4
📏 File size: 12.45 MB
🎉 Video ready: output/synthesia_video_20250708_143022.mp4

📋 Generated Script Preview:
==================================================
Hey everyone, and welcome! Today we're diving into the exciting world of Artific
ial Intelligence and Machine Learning – or AI and ML, as they're often called...
==================================================
✅ Process completed successfully!
```

## 🤝 Contributing

This is a proof-of-concept project with a specific minimal scope. Please refer to `PROJECT_PLAN.md` for detailed development guidelines and current status.

Current development priorities:
1. **Phase 4**: DeepBrainAI integration for alternative video generation
2. **Phase 5**: Complete end-to-end testing and documentation
3. **Optimization**: Performance improvements and error handling enhancements

## 📄 License

This project is for educational and demonstration purposes.

---

**🎯 Goal**: Simple text-to-video conversion with minimal complexity  
**⚡ Status**: Phases 1-3 Complete, Phase 4 (DeepBrainAI) In Planning  
**📅 Last Updated**: July 8, 2025
