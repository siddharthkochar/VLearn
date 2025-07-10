# 📖 VLearn V2 - Complete User Guide

## 🚀 Quick Start Guide

### System Requirements
- Windows 10/11 or macOS/Linux
- .NET 8 Runtime installed
- Internet connection for API calls
- At least 100MB free disk space for video output

### API Key Requirements
You need **all three API keys** for full functionality:

1. **Google Gemini API** - For script generation (required)
2. **DeepBrainAI API** - For primary video generation (required) 
3. **Synthesia API** - For fallback video generation (required)

## 🔧 Installation & Setup

### Step 1: Clone and Build
```bash
git clone https://github.com/siddharthkochar/VLearn.git
cd VLearn/VLearn.Console
dotnet build
```

### Step 2: Configure API Keys
Edit `appsettings.json` with your API keys:

```json
{
  "GeminiApi": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta"
  },
  "SynthesiaApi": {
    "ApiKey": "YOUR_SYNTHESIA_API_KEY_HERE", 
    "BaseUrl": "https://api.synthesia.io/v2"
  },
  "DeepBrainApi": {
    "ApiKey": "YOUR_DEEPBRAIN_API_KEY_HERE",
    "BaseUrl": "https://v2.aistudios.com/api/odin"
  }
}
```

### Step 3: Test Installation
```bash
dotnet run -- generate
```

## 📝 Usage Examples

### File Input Mode (Recommended)
Create a text file with your content:

**content.txt**
```
Machine Learning Basics

Machine learning is a subset of artificial intelligence that enables computers to learn from data. Key concepts include supervised learning, unsupervised learning, and reinforcement learning.
```

Run the application:
```bash
dotnet run -- generate content.txt
```

### Interactive Console Mode
```bash
dotnet run -- generate
# Then type your content and press Enter twice to finish
```

### Command Line Help
```bash
dotnet run -- help
# Shows usage information
```

## 🎬 Processing Workflow

### What Happens When You Run VLearn

1. **📖 Input Reading** (1-2 seconds)
   - Reads your text from file or console
   - Validates content length and format

2. **🧠 Script Generation** (5-10 seconds)
   - Sends content to Google Gemini API
   - Converts raw text to structured learning script
   - Generates title and estimates duration

3. **🎥 Video Creation** (3-5 minutes)
   - **Primary**: Attempts DeepBrainAI video generation
   - **Fallback**: If DeepBrainAI fails, uses Synthesia
   - Shows real-time status updates

4. **💾 Download & Save** (10-30 seconds)
   - Downloads completed video file
   - Saves to `output/` folder with timestamp
   - Shows file size and location

### Expected Timeline
- **Total Time**: 4-6 minutes per video
- **Script Generation**: 5-10 seconds
- **Video Processing**: 3-5 minutes (varies by provider)
- **Download**: 10-30 seconds (depends on video size)

## 🔄 Dual Provider System Explained

### Provider Priority
1. **DeepBrainAI** (Primary) - Modern AI avatars, faster processing
2. **Synthesia** (Fallback) - Reliable backup, traditional avatars

### When Failover Occurs
- DeepBrainAI API is down or unreachable
- DeepBrainAI processing fails or times out
- DeepBrainAI returns an error response
- Network issues with DeepBrainAI servers

### Output File Naming
- **DeepBrainAI**: `deepbrain_video_20250710_143022_Your_Title.mp4`
- **Synthesia**: `synthesia_video_20250710_143025_Your_Title.mp4`

## 🎛️ Default Settings Reference

### DeepBrainAI Settings (Primary)
```json
{
  "model": "ysy",           // AI avatar model
  "clothes": "1",           // Default outfit
  "language": "en",         // Auto-detected
  "voice": "default"        // Model's default voice
}
```

### Synthesia Settings (Fallback)
```json
{
  "avatar": "anna_costume1_cameraA",  // Professional female avatar
  "background": "green_screen",       // Chroma key background
  "voice": "default"                  // Avatar's default voice
}
```

## 🛠️ Troubleshooting Guide

### Common Issues

#### "Configuration file not found"
**Problem**: `appsettings.json` not in output directory
**Solution**: 
```bash
cd VLearn.Console
dotnet build
# Ensure appsettings.json is copied to bin/Debug/net8.0/
```

#### "API key is not configured"
**Problem**: Missing or incorrect API keys
**Solution**: 
1. Verify all three API keys are in `appsettings.json`
2. Check for typos or extra spaces
3. Ensure JSON syntax is valid

#### "Both video providers failed"
**Problem**: Network issues or both APIs are down
**Solution**:
1. Check internet connection
2. Verify API keys are valid and active
3. Try again in a few minutes
4. Check API provider status pages

#### "Script generation failed"
**Problem**: Gemini API issues or content too long
**Solution**:
1. Check Gemini API key
2. Reduce input text length (under 2000 characters)
3. Ensure text is in English or supported language

#### "Video processing timed out"
**Problem**: Video generation taking longer than 5 minutes
**Solution**:
1. This is normal for longer content
2. Provider may be experiencing high load
3. Try with shorter text content
4. Wait and try again later

### Performance Tips

#### Optimal Input Length
- **Ideal**: 200-800 characters
- **Maximum**: 2000 characters
- **Minimum**: 50 characters

#### Best Content Types
- Educational explanations
- How-to instructions
- Concept overviews
- Tutorial content
- Technical summaries

#### Avoid
- Very long articles (>2000 chars)
- Non-English content (limited support)
- Code-heavy content
- Tables or complex formatting

## 📊 Output Quality Guidelines

### Video Specifications
- **Format**: MP4 (H.264)
- **Resolution**: Provider default (typically 1080p)
- **Duration**: Based on script length (roughly 1 minute per 150 words)
- **Audio**: Clear synthetic voice narration
- **Size**: Typically 10-20MB per minute

### File Organization
```
VLearn.Console/
├── output/                          # All generated videos
│   ├── deepbrain_video_20250710_143022_AI_Basics.mp4
│   ├── deepbrain_video_20250710_144530_ML_Guide.mp4
│   └── synthesia_video_20250710_145000_Backup_Video.mp4
```

## 🔐 Security & Privacy

### API Key Security
- Store API keys securely in `appsettings.json`
- Never commit API keys to version control
- Use environment variables for production deployment
- Rotate keys regularly as per provider recommendations

### Data Privacy
- Input text is sent to Google Gemini for processing
- Scripts are sent to video providers (DeepBrainAI/Synthesia)
- No data is stored permanently by VLearn
- All processing is real-time with immediate cleanup

### Network Requirements
- HTTPS connections to all API providers
- No special firewall configuration needed
- Standard internet connection sufficient

## 📈 Scaling & Limitations

### Current Limitations
- One video at a time (no batch processing)
- English language optimized
- No video customization options
- No progress persistence (restart = start over)
- No cost tracking or limits

### Scaling Considerations
- API rate limits apply (check provider documentation)
- Video processing time scales with content length
- Storage space required for output files
- Network bandwidth for video downloads

## 🆘 Support & Resources

### API Provider Documentation
- [Google Gemini API](https://ai.google.dev/docs)
- [DeepBrain AI Studios](https://www.aistudios.com/help)
- [Synthesia API](https://docs.synthesia.io/)

### Getting Help
1. Check this troubleshooting guide first
2. Review `PROJECT_PLAN.md` for technical details
3. Verify API provider status pages
4. Test with minimal input to isolate issues

### Sample Test Content
Use this content to test your installation:

```
Artificial Intelligence Overview

AI is the simulation of human intelligence in machines. It includes machine learning, where computers learn from data, and deep learning, which uses neural networks. Common applications include voice assistants, image recognition, and automated decision-making systems.
```

Expected result: 2-3 minute video explaining AI concepts with clear narration.

---

**🎯 This guide covers everything needed to successfully use VLearn V2!**
**⚡ For technical details, see PROJECT_PLAN.md**
**📅 Last Updated: July 10, 2025**
