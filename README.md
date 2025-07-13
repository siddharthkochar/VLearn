# 🎬 VLearn - AI Video Learning Console App

A console application that converts text input into learning videos using Google Gemini API for script generation and HeyGen API for AI avatar video creation. Features multiple script types and precise duration control for customized educational content.

## 🎯 Overview

VLearn is an AI-powered video generation application that demonstrates an automated pipeline for creating educational videos from text content. The application offers **7 different script types** and **precise duration control in seconds** to create personalized learning experiences.

The system uses **HeyGen's AI avatar technology** to create professional-quality educational videos with realistic AI presenters, tailored to your preferred learning style and time constraints.

## 🎬 Features

### 📝 Script Types Available
1. **Standard Educational** - Clear, structured educational format
2. **Storytelling Narrative** - Engaging stories with characters and scenarios
3. **Documentary Style** - Professional, authoritative tone with facts
4. **Step-by-Step Tutorial** - Instructional format with clear steps
5. **Simplified Explainer** - Easy explanations with analogies
6. **Real-World Case Study** - Practical examples and applications
7. **Conversational Style** - Natural dialogue like talking to a friend

### ⏱️ Duration Control
- **Very Short** (10-30 seconds) - Quick concept overview
- **Short** (30 seconds - 2 minutes) - Brief explanation
- **Medium** (2-5 minutes) - Detailed explanation
- **Long** (5-10 minutes) - Comprehensive coverage
- **Custom** (10-1800 seconds) - Precise user-defined duration

### 🎯 Smart Content Adaptation
- AI automatically adjusts content depth based on requested duration
- Word count optimization for natural speech pacing (150 words/minute)
- Type-specific prompts for optimal script generation

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
2. **🎬 Script Type Selection** - Choose from 7 different presentation styles
3. **⏱️ Duration Selection** - Set precise duration in seconds (10s-30min)
4. **🧠 Script Generation** - Converts text to tailored learning script using Google Gemini API
5. **🤖 Video Creation** - Generates video using HeyGen's AI avatar technology
6. **💾 Output** - Downloads MP4 video to local `output/` folder with timestamped naming

### 📋 Interactive Experience
The application provides an interactive experience where you can:
- Select your preferred script type with detailed descriptions
- Choose duration ranges or specify exact seconds
- Add custom instructions for personalized content
- View estimated vs. actual duration feedback

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
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json        # Configuration file with API settings
│   ├── Models/                 # Data models
│   │   ├── InputText.cs        # Input handling model
│   │   ├── Script.cs           # Script model with type and duration tracking
│   │   ├── ScriptGenerationRequest.cs # Request model for script generation
│   │   ├── VideoRequest.cs     # Video request model
│   │   ├── SampleContent.cs    # Sample content for testing
│   │   └── ApiModels.cs        # API response models
│   ├── Services/               # Application services
│   │   ├── InputService.cs     # Input processing and user interaction
│   │   ├── GeminiService.cs    # Gemini API integration with multiple script types
│   │   ├── HeyGenService.cs    # HeyGen API integration
│   │   └── VideoProcessingService.cs # Video workflow management
│   ├── Extensions/             # Extension methods
│   │   └── ScriptTypeExtensions.cs # Script type display helpers
│   └── Configuration/          # Configuration models
│       └── AppSettings.cs      # Settings classes for APIs
├── docs/                       # Documentation
│   ├── PROJECT_PLAN.md         # Detailed project plan
│   └── [Other documentation files]
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

### ✅ Phase 4: Enhanced User Experience & Multiple Script Types (COMPLETED)
- [x] Multiple script type implementation (7 different styles)
- [x] Precise duration control in seconds (10s-30min range)
- [x] Interactive user experience with guided selection
- [x] Smart content adaptation based on duration and type
- [x] Enhanced feedback with estimated vs actual duration
- [x] Custom instructions support for personalized content

### ✅ Phase 5: Documentation & Code Organization (COMPLETED)
- [x] Complete pipeline integration testing
- [x] Build verification and configuration validation
- [x] HeyGen video generation workflow testing
- [x] Comprehensive usage documentation update
- [x] Code organization and cleanup

## 🚫 Limitations

Current limitations:
- ❌ Requires manual API key configuration
- ❌ Supports only text input (no PDFs, URLs, etc.)
- ❌ Processes one video at a time
- ❌ Uses fixed HeyGen avatar and voice settings
- ❌ No video preview before generation

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
🎬 VLearn - AI Video Learning Console App
==================================================
📖 Processing input...
✅ Input received from: sample-input.txt
📝 Content length: 673 characters

📝 Script Generation Options
==================================================

🎬 Choose script type:
1. Standard Educational - Clear, educational format with structured content
2. Storytelling Narrative - Engaging narrative with characters and scenarios
3. Documentary Style - Professional, authoritative tone with facts
4. Step-by-Step Tutorial - Step-by-step instructional format
5. Simplified Explainer - Simplified explanations with analogies
6. Real-World Case Study - Real-world examples and practical applications
7. Conversational Style - Natural dialogue style like talking to a friend

Enter your choice (1-7): 5
✅ Selected: Simplified Explainer

⏱️ Video duration options:
1. Very Short (10-30 seconds) - Quick concept overview
2. Short (30 seconds - 2 minutes) - Brief explanation
3. Medium (2-5 minutes) - Detailed explanation
4. Long (5-10 minutes) - Comprehensive coverage
5. Custom duration in seconds
Pro Tip I: Choose custom 10s for quick generation
Pro Tip II: Remember! longer the video, longer the processing time

Enter your choice (1-5): 3
Enter duration in seconds (120-300s for medium video): 180
✅ Selected: 180 seconds (3.0 minutes)

📋 Custom Instructions (optional):
Add any specific requirements or preferences for your script.
Press Enter to skip, or type your instructions:

🧠 Generating script with Gemini...
🔗 Calling Gemini API for Explainer script (180s / 3.0min)...
✅ Script generated successfully!
📝 Script type: Simplified Explainer
📝 Script title: Understanding AI and Machine Learning Made Simple
📝 Requested duration: 180 seconds (3.0 minutes)
⏱️ Estimated actual duration: 185 seconds (3.1 minutes)

🎥 Creating video with HeyGen...
🎉 Video ready: output/heygen_video_20250713_143022_Understanding_AI_and_Machine_Learning_Made_Simple.mp4
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
- Avatar and voice customization options
- Multiple video provider support  
- Batch processing capabilities
- Advanced error recovery and retry logic
- Video preview and editing capabilities
- Integration with learning management systems

## 📄 License

This project is for educational and demonstration purposes.

---

**🎯 Goal**: Customizable text-to-video conversion with multiple script types and precise duration control  
**⚡ Status**: Enhanced Features Complete - Production Ready ✅  
**📅 Last Updated**: July 13, 2025
