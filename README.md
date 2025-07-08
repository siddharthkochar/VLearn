# 🎬 VLearn V2 - AI Video Learning Console App

A minimal console application that converts text input into learning videos using Google Gemini API for script generation and Synthesia API for video creation.

## 🎯 Overview

VLearn V2 is a simplified proof-of-concept application that demonstrates an automated pipeline for creating educational videos from text content. The application follows a bare minimum approach with no customizations, no logging, and uses default values for all video generation parameters.

## 🚀 Quick Start

### Prerequisites
- .NET 8 Runtime
- Google Gemini API key
- Synthesia API key

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
2. **🧠 Script Generation** - Converts text to learning script using Google Gemini API *(Phase 2 - In Development)*
3. **🎬 Video Creation** - Generates video using Synthesia API with default settings *(Phase 3 - In Development)*
4. **💾 Output** - Downloads MP4 video to local `output/` folder

## 🎛️ Default Settings

- **Avatar**: `anna_costume1_cameraA`
- **Background**: `green_screen`
- **Voice**: Default Synthesia voice for selected avatar
- **Output Format**: MP4 video file
- **Output Location**: `output/video_YYYYMMDD_HHMMSS.mp4`

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
│   │   └── InputService.cs     # Input processing service
│   └── Configuration/          # Configuration models
│       └── AppSettings.cs      # Settings classes
├── PROJECT_PLAN_V2.md          # Detailed project plan
├── PROJECT_PLAN.md             # Original project plan (V1)
└── README.md                   # This file
```

## 🛠️ Technology Stack

- **Framework**: .NET 8 Console Application
- **AI Integration**: Google Gemini API (REST)
- **Video Generation**: Synthesia API (REST)
- **Dependencies**: Minimal - HTTP client, JSON serialization, configuration management

## 📊 Development Status

### ✅ Phase 1: Basic Console Application (COMPLETED)
- [x] Console application with dependency injection
- [x] File and interactive input handling
- [x] Basic configuration system
- [x] Core data models

### 🔄 Phase 2: Google Gemini Integration (IN PROGRESS)
- [ ] Gemini API client implementation
- [ ] Script generation from text input
- [ ] Prompt template for educational content

### 📋 Phase 3: Synthesia Integration (PLANNED)
- [ ] Synthesia API client
- [ ] Video creation with default parameters
- [ ] Video status polling and download

### 🎯 Phase 4: End-to-End Testing (PLANNED)
- [ ] Complete pipeline integration
- [ ] Error handling and user feedback
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

🧠 Generating script with Gemini... (Phase 2 - Not implemented yet)
🎬 Creating video with Synthesia... (Phase 3 - Not implemented yet)

📋 Input Preview:
------------------------------
Artificial Intelligence and Machine Learning
AI and ML are transformative technologies that are reshaping how we solve comple
x problems. Machine learning allows computers to learn patterns from dat...
------------------------------
✅ Process completed successfully!
```

## 🤝 Contributing

This is a proof-of-concept project with a specific minimal scope. Please refer to `PROJECT_PLAN_V2.md` for detailed development guidelines and current status.

## 📄 License

This project is for educational and demonstration purposes.

---

**🎯 Goal**: Simple text-to-video conversion with minimal complexity  
**⚡ Status**: Phase 1 Complete, Phase 2 In Development  
**📅 Last Updated**: July 8, 2025
