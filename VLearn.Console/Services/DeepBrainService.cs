using VLearn.Console.Models;
using VLearn.Console.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace VLearn.Console.Services;

/// <summary>
/// Service to interact with DeepBrainAI API for video generation
/// </summary>
public interface IDeepBrainService
{
    Task<ApiResponse<DeepBrainVideoResponse>> CreateVideoAsync(Script script);
    Task<ApiResponse<DeepBrainProjectResponse>> GetProjectStatusAsync(string projectKey);
    Task<ApiResponse<byte[]>> DownloadVideoAsync(string downloadUrl);
}

public class DeepBrainService : IDeepBrainService
{
    private readonly HttpClient _httpClient;
    private readonly DeepBrainApiSettings _settings;

    public DeepBrainService(HttpClient httpClient, IOptions<AppSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value.DeepBrainApi;
    }

    public async Task<ApiResponse<DeepBrainVideoResponse>> CreateVideoAsync(Script script)
    {
        try
        {
            if (string.IsNullOrEmpty(_settings.ApiKey))
            {
                return new ApiResponse<DeepBrainVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "DeepBrainAI API key is not configured. Please add your API key to appsettings.json",
                    StatusCode = 400
                };
            }

            var videoRequest = CreateVideoRequest(script);
            var json = JsonSerializer.Serialize(videoRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            System.Console.WriteLine("🤖 Creating video with DeepBrainAI...");
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/simple/video");
            request.Headers.Add("Authorization", _settings.ApiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine($"📡 API Response Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<DeepBrainVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"DeepBrainAI API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var videoResponse = JsonSerializer.Deserialize<DeepBrainVideoResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (videoResponse == null || !videoResponse.Success)
            {
                return new ApiResponse<DeepBrainVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "DeepBrainAI API returned unsuccessful response",
                    StatusCode = 500
                };
            }

            return new ApiResponse<DeepBrainVideoResponse>
            {
                IsSuccess = true,
                Data = videoResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<DeepBrainVideoResponse>
            {
                IsSuccess = false,
                ErrorMessage = $"Error calling DeepBrainAI API: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<ApiResponse<DeepBrainProjectResponse>> GetProjectStatusAsync(string projectKey)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/simple/video/{projectKey}");
            request.Headers.Add("Authorization", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<DeepBrainProjectResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"DeepBrainAI API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var projectResponse = JsonSerializer.Deserialize<DeepBrainProjectResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (projectResponse == null)
            {
                return new ApiResponse<DeepBrainProjectResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "No response received from DeepBrainAI API",
                    StatusCode = 500
                };
            }

            return new ApiResponse<DeepBrainProjectResponse>
            {
                IsSuccess = true,
                Data = projectResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<DeepBrainProjectResponse>
            {
                IsSuccess = false,
                ErrorMessage = $"Error checking project status: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<ApiResponse<byte[]>> DownloadVideoAsync(string downloadUrl)
    {
        try
        {
            System.Console.WriteLine("💾 Downloading DeepBrainAI video...");
            
            var response = await _httpClient.GetAsync(downloadUrl);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<byte[]>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to download video: {response.StatusCode}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var videoBytes = await response.Content.ReadAsByteArrayAsync();

            return new ApiResponse<byte[]>
            {
                IsSuccess = true,
                Data = videoBytes,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<byte[]>
            {
                IsSuccess = false,
                ErrorMessage = $"Error downloading video: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    private DeepBrainVideoRequest CreateVideoRequest(Script script)
    {
        return new DeepBrainVideoRequest
        {
            Language = DetectLanguage(script.Content),
            Text = script.Content,
            Model = "ysy", // Default AI model as specified
            Clothes = "1"  // Default clothes configuration
        };
    }

    private string DetectLanguage(string text)
    {
        // Simple language detection - can be enhanced later
        // For now, assume English for all content
        return "en";
    }
}
