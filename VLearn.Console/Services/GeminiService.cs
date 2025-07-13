using VLearn.Console.Models;
using VLearn.Console.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace VLearn.Console.Services;

/// <summary>
/// Service to interact with Google Gemini API for script generation
/// </summary>
public interface IGeminiService
{
    Task<ApiResponse<Script>> GenerateScriptAsync(string inputText);
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
        try
        {
            if (string.IsNullOrEmpty(_settings.ApiKey))
            {
                return new ApiResponse<Script>
                {
                    IsSuccess = false,
                    ErrorMessage = "Gemini API key is not configured. Please add your API key to appsettings.json",
                    StatusCode = 400
                };
            }

            var prompt = CreateLearningScriptPrompt(inputText);
            var requestBody = CreateGeminiRequest(prompt);
            var json = JsonSerializer.Serialize(requestBody);
            
            var url = $"{_settings.BaseUrl}/models/{_model}:generateContent?key={_settings.ApiKey}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Console.WriteLine("🔗 Calling Gemini API...");
            var response = await _httpClient.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine($"📡 API Response Status: {response.StatusCode}");
            System.Console.WriteLine($"📄 Response Length: {responseJson.Length} characters");

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<Script>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Gemini API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text == null)
            {
                return new ApiResponse<Script>
                {
                    IsSuccess = false,
                    ErrorMessage = "No content received from Gemini API",
                    StatusCode = 500
                };
            }

            var scriptText = geminiResponse.Candidates.First().Content.Parts.First().Text;
            var script = ParseScriptFromResponse(scriptText, inputText);

            return new ApiResponse<Script>
            {
                IsSuccess = true,
                Data = script,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Script>
            {
                IsSuccess = false,
                ErrorMessage = $"Error calling Gemini API: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    private string CreateLearningScriptPrompt(string inputText)
    {
        return $@"You are an expert educational content creator. Convert the following text into a clear, engaging video script for learning purposes.

REQUIREMENTS:
- Create a script suitable for a 2 second educational video
- Use conversational, clear language appropriate for video narration
- Structure the content with a brief introduction, main points, and conclusion
- Make it engaging and easy to follow when spoken aloud
- Keep sentences concise and suitable for video pacing
- Do not include camera directions, scene descriptions, or technical video instructions
- Focus only on the spoken content that will be narrated

INPUT TEXT:
{inputText}

OUTPUT INSTRUCTIONS:
- Provide only the script text that should be spoken
- Use natural speech patterns and transitions
- Make it educational and informative
- Ensure the content flows well when read aloud

SCRIPT:";
    }

    private object CreateGeminiRequest(string prompt)
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
                maxOutputTokens = 1024
            }
        };
    }

    private Script ParseScriptFromResponse(string scriptText, string originalText)
    {
        // Clean up the script text
        var cleanScript = scriptText.Trim();
        
        // Remove any potential markdown formatting
        cleanScript = cleanScript.Replace("**", "").Replace("*", "");
        
        // Estimate duration (roughly 150 words per minute for natural speech)
        var wordCount = cleanScript.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        var estimatedDurationSeconds = (int)Math.Ceiling(wordCount / 2.5); // 150 words/min = 2.5 words/sec

        // Extract a title from the first line or generate one
        var lines = cleanScript.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var title = lines.Length > 0 && lines[0].Length < 100 
            ? lines[0].Trim() 
            : "Learning Video Script";

        return new Script
        {
            Content = cleanScript,
            Title = title,
            EstimatedDurationSeconds = estimatedDurationSeconds,
            GeneratedAt = DateTime.UtcNow
        };
    }
}
