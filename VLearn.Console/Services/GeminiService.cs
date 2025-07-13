using VLearn.Console.Models;
using VLearn.Console.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace VLearn.Console.Services;

public interface IGeminiService
{
    Task<ApiResponse<Script>> GenerateScriptAsync(string inputText);
    Task<ApiResponse<Script>> GenerateScriptAsync(ScriptGenerationRequest request);
}

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiApiSettings _settings;
    private readonly string _model = "gemini-1.5-flash"; // Using the latest model

    public GeminiService(HttpClient httpClient, IOptions<AppSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value.GeminiApi;
    }

    public async Task<ApiResponse<Script>> GenerateScriptAsync(string inputText)
    {
        var request = new ScriptGenerationRequest
        {
            InputText = inputText,
            ScriptType = ScriptType.Standard,
            DurationSeconds = 120 // Default 2 minutes
        };
        
        return await GenerateScriptAsync(request);
    }

    public async Task<ApiResponse<Script>> GenerateScriptAsync(ScriptGenerationRequest request)
    {
        if (string.IsNullOrEmpty(_settings.ApiKey))
        {
            return new ApiResponse<Script>(
                IsSuccess: false,
                Data: null,
                ErrorMessage: "Gemini API key is not configured. Please add your API key to appsettings.json",
                StatusCode: 400
            );
        }

        var prompt = CreateScriptPrompt(request);
        var requestBody = CreateGeminiRequest(prompt);
        var json = JsonSerializer.Serialize(requestBody);
            
        var url = $"{_settings.BaseUrl}/models/{_model}:generateContent?key={_settings.ApiKey}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        WriteLine($"🔗 Calling Gemini API for {request.ScriptType} script ({request.DurationSeconds}s / {request.DurationMinutes:F1}min)...");
        var response = await _httpClient.PostAsync(url, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        WriteLine($"📡 API Response Status: {response.StatusCode}");
        WriteLine($"📄 Response Length: {responseJson.Length} characters");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<Script>(
                IsSuccess: false,
                Data: null,
                ErrorMessage: $"Gemini API error: {response.StatusCode} - {responseJson}",
                StatusCode: (int)response.StatusCode
            );
        }

        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text == null)
        {
            return new ApiResponse<Script>(
                IsSuccess: false,
                Data: null,
                ErrorMessage: "No content received from Gemini API",
                StatusCode: 500
            );
        }

        var scriptText = geminiResponse.Candidates.First().Content.Parts.First().Text;
        var script = ParseScriptFromResponse(scriptText, request);

        return new ApiResponse<Script>(
            IsSuccess: true,
            Data: script,
            ErrorMessage: string.Empty,
            StatusCode: 200
        );
    }

    private static string CreateScriptPrompt(ScriptGenerationRequest request)
    {
        var basePrompt = GetScriptTypePrompt(request.ScriptType);
        var durationGuidance = GetDurationGuidance(request.DurationSeconds);
        
        return $@"{basePrompt}

DURATION REQUIREMENTS:
{durationGuidance}

INPUT TEXT:
{request.InputText}

{(!string.IsNullOrEmpty(request.CustomInstructions) ? $"ADDITIONAL INSTRUCTIONS:\n{request.CustomInstructions}\n\n" : "")}OUTPUT INSTRUCTIONS:
- Provide only the script text that should be spoken
- Use natural speech patterns and transitions
- Make it educational and informative
- Ensure the content flows well when read aloud
- Target exactly {request.DurationSeconds} seconds ({request.DurationMinutes:F1} minutes) of speaking time

SCRIPT:";
    }

    private static string GetScriptTypePrompt(ScriptType scriptType)
    {
        return scriptType switch
        {
            ScriptType.Storytelling => @"You are an expert storyteller and educational content creator. Convert the following text into an engaging narrative-style video script that teaches through story. 

STORYTELLING REQUIREMENTS:
- Create a compelling narrative with a clear beginning, middle, and end
- Use characters, scenarios, or situations to illustrate the concepts
- Make abstract ideas concrete through relatable examples
- Include emotional elements to enhance engagement and retention
- Structure like a story while maintaining educational value",

            ScriptType.Documentary => @"You are a professional documentary scriptwriter and educational expert. Convert the following text into an authoritative, fact-based video script in documentary style.

DOCUMENTARY REQUIREMENTS:
- Use a professional, authoritative tone throughout
- Present information with credibility and gravitas
- Include relevant facts, statistics, or expert perspectives where appropriate
- Structure with clear segments and smooth transitions
- Maintain objectivity while being engaging",

            ScriptType.Tutorial => @"You are an expert instructor and tutorial creator. Convert the following text into a clear, step-by-step instructional video script.

TUTORIAL REQUIREMENTS:
- Break down complex concepts into digestible steps
- Use clear, actionable language
- Provide practical examples and applications
- Structure with logical progression from basic to advanced
- Include helpful tips and common pitfalls to avoid",

            ScriptType.Explainer => @"You are a master at simplifying complex topics. Convert the following text into an explainer video script that makes difficult concepts easy to understand.

EXPLAINER REQUIREMENTS:
- Break down complex ideas into simple, digestible parts
- Use analogies and metaphors to clarify difficult concepts
- Focus on the 'why' and 'how' behind the information
- Use conversational, approachable language
- Build understanding progressively",

            ScriptType.CaseStudy => @"You are an expert at presenting real-world applications and case studies. Convert the following text into a case study video script that demonstrates practical applications.

CASE STUDY REQUIREMENTS:
- Present real-world examples and scenarios
- Show practical applications of the concepts
- Include specific examples, situations, or implementations
- Demonstrate cause and effect relationships
- Connect theory to practice effectively",

            ScriptType.Conversational => @"You are an expert at creating engaging dialogues for educational content. Convert the following text into a conversational video script that feels like a natural discussion.

CONVERSATIONAL REQUIREMENTS:
- Write in a natural, dialogue-like style as if speaking to a friend
- Use questions and answers to guide the learning
- Include natural speech patterns and transitions
- Make it feel interactive and engaging
- Use a warm, approachable tone throughout",

            _ => @"You are an expert educational content creator. Convert the following text into a clear, engaging video script for learning purposes.

STANDARD REQUIREMENTS:
- Use conversational, clear language appropriate for video narration
- Structure the content with a brief introduction, main points, and conclusion
- Make it engaging and easy to follow when spoken aloud
- Keep sentences concise and suitable for video pacing"
        };
    }

    private static string GetDurationGuidance(int durationSeconds)
    {
        var durationMinutes = durationSeconds / 60.0;
        var wordsNeeded = (int)(durationSeconds * 2.5); // 150 words per minute = 2.5 words per second
        
        return $@"- Target approximately {wordsNeeded} words for {durationSeconds} seconds ({durationMinutes:F1} minutes) of content
- Average speaking pace is 150 words per minute (2.5 words per second)
- Adjust content depth and detail to fit the time constraint
- For very short videos (10-30s): Focus on one key concept only
- For short videos (30s-2min): Focus on key concepts with brief explanations
- For medium videos (2-5min): Include examples and detailed explanations
- For longer videos (5min+): Add comprehensive explanations, multiple examples, and deeper context";
    }

    private static object CreateGeminiRequest(string prompt)
    {
        return new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.7,
                topK = 40,
                topP = 0.95,
                maxOutputTokens = 2048 // Increased for longer scripts
            }
        };
    }

    private static Script ParseScriptFromResponse(string scriptText, ScriptGenerationRequest request)
    {
        // Clean up the script text
        var cleanScript = scriptText.Trim();
        
        // Remove any potential markdown formatting
        cleanScript = cleanScript.Replace("**", "").Replace("*", "");
        
        // Calculate actual duration based on word count
        var wordCount = cleanScript.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        var estimatedDurationSeconds = (int)Math.Ceiling(wordCount / 2.5); // 150 words/min = 2.5 words/sec

        // Extract a title from the first line or generate one based on script type
        var lines = cleanScript.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var title = lines.Length > 0 && lines[0].Length < 100 
            ? lines[0].Trim() 
            : $"{request.ScriptType} Learning Script";

        return new Script
        {
            Content = cleanScript,
            Title = title,
            EstimatedDurationSeconds = estimatedDurationSeconds,
            GeneratedAt = DateTime.UtcNow,
            Type = request.ScriptType,
            RequestedDurationSeconds = request.DurationSeconds
        };
    }
}
