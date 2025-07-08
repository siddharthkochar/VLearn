using VLearn.Console.Models;
using VLearn.Console.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace VLearn.Console.Services;

/// <summary>
/// Service to interact with Synthesia API for video generation
/// </summary>
public interface ISynthesiaService
{
    Task<ApiResponse<VideoResponse>> CreateVideoAsync(Script script);
    Task<ApiResponse<VideoResponse>> GetVideoStatusAsync(string videoId);
    Task<ApiResponse<byte[]>> DownloadVideoAsync(string downloadUrl);
}

public class SynthesiaService : ISynthesiaService
{
    private readonly HttpClient _httpClient;
    private readonly SynthesiaApiSettings _settings;

    public SynthesiaService(HttpClient httpClient, IOptions<AppSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value.SynthesiaApi;
    }

    public async Task<ApiResponse<VideoResponse>> CreateVideoAsync(Script script)
    {
        try
        {
            if (string.IsNullOrEmpty(_settings.ApiKey))
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "Synthesia API key is not configured. Please add your API key to appsettings.json",
                    StatusCode = 400
                };
            }

            var videoRequest = CreateVideoRequest(script);
            var json = JsonSerializer.Serialize(videoRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            System.Console.WriteLine("🎬 Creating video with Synthesia...");
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/videos");
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine($"📡 API Response Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Synthesia API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var videoResponse = JsonSerializer.Deserialize<VideoResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (videoResponse == null)
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "No response received from Synthesia API",
                    StatusCode = 500
                };
            }

            return new ApiResponse<VideoResponse>
            {
                IsSuccess = true,
                Data = videoResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VideoResponse>
            {
                IsSuccess = false,
                ErrorMessage = $"Error calling Synthesia API: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<ApiResponse<VideoResponse>> GetVideoStatusAsync(string videoId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/videos/{videoId}");
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Synthesia API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var videoResponse = JsonSerializer.Deserialize<VideoResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (videoResponse == null)
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "No response received from Synthesia API",
                    StatusCode = 500
                };
            }

            return new ApiResponse<VideoResponse>
            {
                IsSuccess = true,
                Data = videoResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VideoResponse>
            {
                IsSuccess = false,
                ErrorMessage = $"Error checking video status: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<ApiResponse<byte[]>> DownloadVideoAsync(string downloadUrl)
    {
        try
        {
            System.Console.WriteLine("💾 Downloading video...");
            
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

    private SynthesiaVideoRequest CreateVideoRequest(Script script)
    {
        return new SynthesiaVideoRequest
        {
            Test = true, // Keep as test mode for now
            Title = script.Title,
            Input = new List<SynthesiaInput>
            {
                new SynthesiaInput
                {
                    Avatar = new SynthesiaAvatar
                    {
                        Avatar = "anna_costume1_cameraA" // Default avatar as specified
                    },
                    Background = new SynthesiaBackground
                    {
                        Background = "green_screen" // Default background as specified
                    },
                    ScriptText = script.Content
                }
            }
        };
    }
}
