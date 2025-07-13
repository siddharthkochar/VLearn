using VLearn.Console.Models;
using VLearn.Console.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace VLearn.Console.Services;

/// <summary>
/// Service to interact with HeyGen API for video generation
/// </summary>
public interface IHeyGenService
{
    Task<ApiResponse<HeyGenVideoResponse>> CreateVideoAsync(Script script);
    Task<ApiResponse<HeyGenVideoStatusResponse>> GetVideoStatusAsync(string videoId);
    Task<ApiResponse<byte[]>> DownloadVideoAsync(string downloadUrl);
}

public class HeyGenService : IHeyGenService
{
    private readonly HttpClient _httpClient;
    private readonly HeyGenApiSettings _settings;

    public HeyGenService(HttpClient httpClient, IOptions<AppSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value.HeyGenApi;
    }

    public async Task<ApiResponse<HeyGenVideoResponse>> CreateVideoAsync(Script script)
    {
        try
        {
            if (string.IsNullOrEmpty(_settings.ApiKey))
            {
                return new ApiResponse<HeyGenVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "HeyGen API key is not configured. Please add your API key to appsettings.json",
                    StatusCode = 400
                };
            }

            var videoRequest = CreateVideoRequest(script);
            var json = JsonSerializer.Serialize(videoRequest);

            System.Console.WriteLine("🎬 Creating video with HeyGen...");
            System.Console.WriteLine($"🔍 Request JSON: {json}");
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/video/generate");
            request.Headers.Add("X-Api-Key", _settings.ApiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine($"📡 API Response Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<HeyGenVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"HeyGen API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var videoResponse = JsonSerializer.Deserialize<HeyGenVideoResponse>(responseJson);

            if (videoResponse == null || videoResponse.Code != 100)
            {
                return new ApiResponse<HeyGenVideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"HeyGen API returned error: {videoResponse?.Message ?? "Unknown error"}",
                    StatusCode = 500
                };
            }

            return new ApiResponse<HeyGenVideoResponse>
            {
                IsSuccess = true,
                Data = videoResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<HeyGenVideoResponse>
            {
                IsSuccess = false,
                ErrorMessage = $"Error calling HeyGen API: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    public async Task<ApiResponse<HeyGenVideoStatusResponse>> GetVideoStatusAsync(string videoId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/v1/video_status.get?video_id={videoId}");
            request.Headers.Add("X-Api-Key", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<HeyGenVideoStatusResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"HeyGen API error: {response.StatusCode} - {responseJson}",
                    StatusCode = (int)response.StatusCode
                };
            }

            var statusResponse = JsonSerializer.Deserialize<HeyGenVideoStatusResponse>(responseJson);

            if (statusResponse == null || statusResponse.Code != 100)
            {
                return new ApiResponse<HeyGenVideoStatusResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"HeyGen API error: {statusResponse?.Message ?? "Unknown error"}",
                    StatusCode = 500
                };
            }

            return new ApiResponse<HeyGenVideoStatusResponse>
            {
                IsSuccess = true,
                Data = statusResponse,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<HeyGenVideoStatusResponse>
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
            System.Console.WriteLine("💾 Downloading HeyGen video...");
            
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

    private HeyGenVideoRequest CreateVideoRequest(Script script)
    {
        return new HeyGenVideoRequest
        {
            VideoInputs = new List<HeyGenVideoInput>
            {
                new HeyGenVideoInput
                {
                    Character = new HeyGenCharacter
                    {
                        Type = "avatar",
                        AvatarId = "Abigail_expressive_2024112501",
                        AvatarStyle = "normal"
                    },
                    Voice = new HeyGenVoice
                    {
                        Type = "text",
                        InputText = script.Content,
                        VoiceId = "73c0b6a2e29d4d38aca41454bf58c955",
                        Speed = 1.1
                    }
                }
            },
            Dimension = new HeyGenDimension
            {
                Width = 1280,
                Height = 720
            }
        };
    }
}
