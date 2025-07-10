# 🎬 VLearn V2 - Simplified AI Video Learning Console App

## 📋 Project Overview
**🎯 Goal:** Build a minimal console application that accepts text input and generates learning videos using Google Gemini API for script generation and Synthesia API for video creation.

**🔧 Approach:** Bare minimum functionality with no customizations, no logging, and default values for all video generation parameters.

**⏱️ Timeline:** 1-2 weeks (estimated)

**🔗 Key Technologies:** .NET 8 • Console Application • Google Gemini API • Synthesia API • C#

---

## 🎯 Core Requirements (Minimal MVP)

### 🏗️ Phase 1: Basic Console Application Setup (2-3 days) ✅ COMPLETED

#### 🔧 Application Structure
- [x] **🏭 Create simple .NET 8 console application** *(1 day)* ✅
  - [x] ⚡ Basic console app with minimal dependencies
  - [x] 📁 Simple folder structure (no complex architecture)
  - [x] 📦 Only essential NuGet packages

#### 📋 Core Models
- [x] **📝 Define basic data models** *(1 day)* ✅
  - [x] 📄 Input text model
  - [x] 📜 Script model for Gemini output
  - [x] 🎥 Video request model for Synthesia
  - [x] 📊 Simple response models

#### ⌨️ Single Command Interface
- [x] **🔷 Implement unified command** *(1 day)* ✅
  - [x] 📝 Accept text input via console prompt
  - [x] 📄 Accept text input via file path argument
  - [x] 🚀 Single command: `vlearn generate [optional-file-path]`

---

### 🧠 Phase 2: Google Gemini Integration (2-3 days) ✅ COMPLETED

#### 🤖 Gemini API Setup
- [x] **🔌 Google Gemini API client** *(1 day)* ✅
  - [x] 🔐 API key configuration
  - [x] 🌐 HTTP client setup for Gemini API
  - [x] ⚠️ Basic error handling

#### 📝 Script Generation
- [x] **🧮 Text-to-script conversion** *(1 day)* ✅
  - [x] 📋 Simple prompt template for learning content
  - [x] 🎬 Format output as script suitable for Synthesia
  - [x] ✅ Basic validation of generated script

#### 🔄 Integration Testing
- [x] **🧪 Basic testing** *(1 day)* ✅
  - [x] 📝 Test with sample text inputs
  - [x] ✅ Verify script quality and format
  - [x] 🔧 Adjust prompts if needed

---

### 🎥 Phase 3: Synthesia Integration (3-4 days)

#### 🎭 Synthesia API Setup
- [x] **🔌 Synthesia API client** *(1 day)* ✅
  - [x] 🔐 API key configuration and authentication
  - [x] 🌐 HTTP client for video creation
  - [x] 📊 Basic status checking functionality

#### 🎬 Video Generation
- [x] **🔄 Script to video conversion** *(2 days)* ✅
  - [x] 🗺️ Map script text to Synthesia video request
  - [x] 🎭 Use default avatar (anna_costume1_cameraA)
  - [x] 🎨 Use default background (green_screen)
  - [x] ⏱️ Implement polling for video completion

#### 💾 Video Download
- [x] **📥 Download completed video** *(1 day)* ✅
  - [x] 🔄 Poll video status until complete
  - [x] 📁 Download MP4 file to local directory
  - [x] 📝 Simple file naming convention

---

### 🤖 Phase 4: DeepBrainAI Integration (3-4 days) ✅ COMPLETED

#### 🤖 DeepBrainAI API Setup
- [x] **🔌 DeepBrainAI API client** *(1 day)* ✅
  - [x] 🔐 API key configuration and authentication
  - [x] 🌐 HTTP client for AI Studios video creation
  - [x] 📊 Basic status checking functionality

#### 🎬 AI Video Generation
- [x] **🔄 Script to AI video conversion** *(2 days)* ✅
  - [x] 🗺️ Map script text to DeepBrainAI video request
  - [x] 🎭 Use default AI model (ysy)
  - [x] 👔 Use default clothes configuration ("1")
  - [x] 🌍 Configure language settings (en for English)
  - [x] ⏱️ Implement polling for video completion

#### 💾 AI Video Download
- [x] **📥 Download completed AI video** *(1 day)* ✅
  - [x] 🔄 Poll video status using project key
  - [x] 📁 Download MP4 file to local directory
  - [x] 📝 Enhanced file naming convention with provider prefix

#### 🔄 Dual Provider System
- [x] **⚡ Dual provider architecture** *(Additional)* ✅
  - [x] 🎯 DeepBrainAI as primary provider
  - [x] 🔄 Synthesia as fallback provider
  - [x] 🛡️ Automatic failover mechanism
  - [x] 📝 Provider-specific file naming

---

### ✅🎯 Phase 5: End-to-End Integration & Testing (2-3 days)

#### 🔗 Complete Pipeline
- [x] **⚡ Integrate all components** *(1 day)* ✅
  - [x] 📝 Text Input → Gemini Script → DeepBrainAI Video (primary)
  - [x] 🔄 Sequential processing with status updates
  - [x] ⚠️ Basic error handling throughout pipeline
  - [x] 🔄 Automatic fallback to Synthesia if DeepBrainAI fails

#### 🧪 Final Testing
- [ ] **✅ End-to-end testing** *(1 day)*
  - [ ] 📄 Test with various text file inputs
  - [ ] ✍️ Test with console text input
  - [ ] 🎥 Verify video generation and download
  - [ ] 🔄 Test provider fallback mechanism

#### 📚 Basic Documentation
- [ ] **📖 Usage documentation** *(1 day)*
  - [ ] 🔧 Setup and configuration instructions
  - [ ] ⌨️ Command usage examples
  - [ ] 🔑 API key configuration guide

---

## 🖥️ Application Design

### 📝 **Command Usage**
```bash
# Interactive mode - prompts for text input
vlearn generate

# File input mode
vlearn generate input.txt

# Console will show:
# 1. "Processing input..."
# 2. "Generating script with Gemini..."
# 3. "Creating video with DeepBrainAI..." (primary provider)
# 4. "Video ready: output/deepbrain_video_timestamp.mp4"
```

### 🔄 **Processing Flow**
1. **📖 Input Processing** - Read text from console or file
2. **🧠 Script Generation** - Send to Gemini API for script creation
3. **🎬 Video Creation** - Send script to DeepBrainAI API (primary) with Synthesia as backup
4. **⏰ Status Polling** - Wait for video completion (3-5 minutes)
5. **💾 Download** - Save MP4 to local output folder

### 📁 **Output Structure**
```
output/
├── deepbrain_video_20250708_143022.mp4    # DeepBrainAI generated video
├── synthesia_video_20250708_143025.mp4    # Synthesia generated video (if used)
```

---

## 🛠️ Technology Stack

### 🔧 Core Application
- **🏗️ Framework:** .NET 8 Console Application
- **📦 Dependencies:** Minimal - only HTTP client and JSON serialization
- **🤖 AI Integration:** Google Gemini API (REST API calls)
- **🎥 Video Generation:** DeepBrainAI API (primary) + Synthesia API (backup)

### 📋 Required NuGet Packages
- `System.Text.Json` - JSON serialization
- `Microsoft.Extensions.Http` - HTTP client
- `Microsoft.Extensions.Configuration` - Configuration management

### 🔑 Configuration
- **📄 appsettings.json** for API keys
- **🔧 Environment variables** support
- **🛡️ No complex configuration - simple key-value pairs**

---

## 🎯 Functional Specifications

### 📝 Input Handling
- **✍️ Console Input:** Prompt user to enter text directly
- **📄 File Input:** Accept file path as command line argument
- **✅ Validation:** Basic text length validation (not empty)
- **🚫 No Support:** Multiple files, complex formats, or batch processing

### 🧠 Script Generation (Gemini)
- **🎯 Purpose:** Convert raw text into structured learning script
- **📋 Prompt Template:** Fixed template optimized for educational content
- **🎭 Output Format:** Plain text script suitable for video narration
- **🚫 No Customization:** Single prompt template, no user modifications

### 🎥 Video Generation (DeepBrainAI Primary + Synthesia Backup)
- **👤 AI Model:** Fixed - `ysy` or default DeepBrainAI model
- **👔 Clothes:** Fixed - default clothes configuration (`"1"`)
- **🌍 Language:** Automatic based on script content (`"en"` for English)
- **🗣️ Voice:** Default DeepBrainAI voice for selected model
- **⏱️ Duration:** Based on script length (automatic)
- **🔄 Fallback:** Synthesia API if DeepBrainAI fails
- **🚫 No Customization:** No model selection, clothes options, or voice changes

### 💾 Output Management
- **📁 Location:** `output/` folder in application directory
- **📝 Naming:** `deepbrain_video_YYYYMMDD_HHMMSS.mp4` (primary) or `synthesia_video_YYYYMMDD_HHMMSS.mp4` (backup)
- **🔄 Overwrite:** New timestamp for each video (no overwrites)
- **🚫 No Features:** No metadata files, no intermediate file saving

---

## ⚙️ Configuration Requirements

### 🔑 API Keys Required
```json
{
  "GeminiApi": {
    "ApiKey": "your-gemini-api-key",
    "BaseUrl": "https://generativelanguage.googleapis.com"
  },
  "SynthesiaApi": {
    "ApiKey": "your-synthesia-api-key",
    "BaseUrl": "https://api.synthesia.io"
  },
  "DeepBrainApi": {
    "ApiKey": "your-deepbrain-api-key",
    "BaseUrl": "https://v2.aistudios.com/api/odin"
  }
}
```

### 🌍 Environment Variables (Alternative)
- `GEMINI_API_KEY`
- `SYNTHESIA_API_KEY`
- `DEEPBRAIN_API_KEY`

---

## 🚧 Limitations & Constraints

### 🚫 Explicitly NOT Supported
- ❌ **Logging:** No logging framework or log files
- ❌ **Customization:** No video customization options
- ❌ **Error Recovery:** No retry logic or advanced error handling
- ❌ **Progress Tracking:** Basic console messages only
- ❌ **Configuration UI:** No interactive configuration setup
- ❌ **Multiple Formats:** Text input only
- ❌ **Batch Processing:** One video at a time
- ❌ **Template System:** Single fixed approach

### ⚠️ Known Limitations
- 🕒 **Processing Time:** 3-5 minutes per video (Synthesia processing)
- 📏 **Text Length:** Limited by Gemini API token limits
- 🔄 **Error Handling:** Basic - application may exit on API failures
- 💰 **Cost:** No cost tracking or limits

---

## 🎯 Success Criteria

### ✅ Functional Requirements
- [ ] 📝 Application accepts text input from console or file
- [ ] 🧠 Successfully generates script using Gemini API
- [ ] 🎥 Successfully creates video using Synthesia API
- [ ] 💾 Downloads completed video to local folder
- [ ] ⌨️ Single command interface works as specified

### 📊 Quality Requirements
- [ ] 🎬 Generated videos are playable MP4 files
- [ ] 📝 Scripts are coherent and relevant to input text
- [ ] ⏱️ Process completes within reasonable time (under 10 minutes)
- [ ] 🔧 Application runs without crashes for valid inputs

### 🛠️ Technical Requirements
- [ ] 🏃‍♂️ Console application runs on Windows
- [ ] 🔑 API keys configurable via settings or environment
- [ ] 📁 Output files created in predictable location
- [ ] 💻 Minimal system requirements (.NET 8 runtime only)

---

## 📋 Development Checklist

### 🏗️ Setup Phase
- [ ] 🆕 Create new .NET 8 console application
- [ ] 📦 Add minimal required NuGet packages
- [ ] 🔧 Setup basic configuration system
- [ ] 🔑 Create configuration templates for API keys

### 🤖 Gemini Integration
- [ ] 🌐 Implement HTTP client for Gemini API
- [ ] 📝 Create prompt template for script generation
- [ ] 🧪 Test API integration with sample text
- [ ] ✅ Validate script output format

### 🎥 Synthesia Integration
- [ ] 🌐 Implement HTTP client for Synthesia API
- [ ] 🔄 Create video request with default parameters
- [ ] ⏰ Implement status polling mechanism
- [ ] 💾 Implement video download functionality

### 🤖 DeepBrainAI Integration
- [ ] 🌐 Implement HTTP client for DeepBrainAI API
- [ ] 🔄 Create video request with default AI model and clothes
- [ ] 🌍 Configure language detection and settings
- [ ] ⏰ Implement status polling using project key
- [ ] 💾 Implement video download functionality

### 🔗 Integration & Testing
- [ ] ⚡ Connect all components in main application flow
- [ ] 🧪 Test complete pipeline with various inputs
- [ ] 📝 Create basic usage documentation
- [ ] ✅ Verify all success criteria are met

---

**📅 Created:** July 8, 2025  
**📊 Project Status:** 📋 Planning Phase - Awaiting Approval  
**🎯 Next Step:** ✅ Confirm requirements and begin implementation

---

## 🚀 Quick Implementation Notes

### 🏁 Day 1 Goals
- [ ] ✅ Console app accepts text input
- [ ] 📄 Can read text from file argument
- [ ] 🔧 Basic configuration system working

### 🏁 Day 3 Goals  
- [ ] 🧠 Gemini API integration complete
- [ ] 📝 Can generate scripts from text input

### 🏁 Day 6 Goals
- [ ] 🎥 Synthesia API integration complete
- [ ] 💾 Can download generated videos

### 🏁 Day 9 Goals
- [ ] 🤖 DeepBrainAI API integration complete
- [ ] 🎬 Can generate videos with AI models
- [ ] 🔄 Dual provider system working

### 🏁 Day 11 Goals
- [ ] 🔗 Complete end-to-end pipeline working
- [ ] 📚 Basic usage documentation ready

---

*🎯 Simple. Focused. Minimal. Let's build the core functionality first!* 🚀
