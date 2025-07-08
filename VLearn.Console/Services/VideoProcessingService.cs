using VLearn.Console.Models;
using VLearn.Console.Services;

namespace VLearn.Console.Services;

/// <summary>
/// Service to handle the complete video processing workflow
/// </summary>
public interface IVideoProcessingService
{
    Task<ApiResponse<string>> ProcessVideoAsync(Script script);
}

public class VideoProcessingService : IVideoProcessingService
{
    private readonly ISynthesiaService _synthesiaService;
    private readonly int _maxPollingAttempts = 60; // 5 minutes with 5-second intervals
    private readonly int _pollingIntervalSeconds = 5;

    public VideoProcessingService(ISynthesiaService synthesiaService)
    {
        _synthesiaService = synthesiaService;
    }

    public async Task<ApiResponse<string>> ProcessVideoAsync(Script script)
    {
        try
        {
            // Step 1: Create video
            System.Console.WriteLine("🎬 Submitting video creation request...");
            var createResponse = await _synthesiaService.CreateVideoAsync(script);
            
            if (!createResponse.IsSuccess)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to create video: {createResponse.ErrorMessage}",
                    StatusCode = createResponse.StatusCode
                };
            }

            var videoId = createResponse.Data!.Id;
            System.Console.WriteLine($"✅ Video creation started. Video ID: {videoId}");
            System.Console.WriteLine("⏰ Video is being processed. This typically takes 3-5 minutes...");

            // Step 2: Poll for completion
            var pollResult = await PollForVideoCompletion(videoId);
            if (!pollResult.IsSuccess)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    ErrorMessage = pollResult.ErrorMessage,
                    StatusCode = pollResult.StatusCode
                };
            }

            var completedVideo = pollResult.Data!;
            
            // Step 3: Download video
            if (string.IsNullOrEmpty(completedVideo.DownloadUrl))
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    ErrorMessage = "Video completed but no download URL provided",
                    StatusCode = 500
                };
            }

            var downloadResult = await DownloadAndSaveVideo(completedVideo.DownloadUrl, script.Title);
            if (!downloadResult.IsSuccess)
            {
                return downloadResult;
            }

            return new ApiResponse<string>
            {
                IsSuccess = true,
                Data = downloadResult.Data,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Error processing video: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    private async Task<ApiResponse<VideoResponse>> PollForVideoCompletion(string videoId)
    {
        for (int attempt = 1; attempt <= _maxPollingAttempts; attempt++)
        {
            System.Console.WriteLine($"🔄 Checking video status... (Attempt {attempt}/{_maxPollingAttempts})");
            
            var statusResponse = await _synthesiaService.GetVideoStatusAsync(videoId);
            
            if (!statusResponse.IsSuccess)
            {
                return new ApiResponse<VideoResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to check video status: {statusResponse.ErrorMessage}",
                    StatusCode = statusResponse.StatusCode
                };
            }

            var video = statusResponse.Data!;
            System.Console.WriteLine($"📊 Video status: {video.Status}");

            switch (video.Status.ToLower())
            {
                case "complete":
                    System.Console.WriteLine("✅ Video processing completed!");
                    return statusResponse;
                
                case "failed":
                case "error":
                    return new ApiResponse<VideoResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Video processing failed on Synthesia platform",
                        StatusCode = 500
                    };
                
                case "in_progress":
                case "queued":
                case "pending":
                    // Continue polling
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
                
                default:
                    System.Console.WriteLine($"⚠️ Unknown status: {video.Status}. Continuing to poll...");
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
            }
        }

        return new ApiResponse<VideoResponse>
        {
            IsSuccess = false,
            ErrorMessage = $"Video processing timed out after {_maxPollingAttempts * _pollingIntervalSeconds / 60} minutes",
            StatusCode = 408
        };
    }

    private async Task<ApiResponse<string>> DownloadAndSaveVideo(string downloadUrl, string title)
    {
        try
        {
            // Download video
            var downloadResponse = await _synthesiaService.DownloadVideoAsync(downloadUrl);
            
            if (!downloadResponse.IsSuccess)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    ErrorMessage = downloadResponse.ErrorMessage,
                    StatusCode = downloadResponse.StatusCode
                };
            }

            // Create output directory
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Generate filename
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var safeTitle = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));
            if (safeTitle.Length > 50)
            {
                safeTitle = safeTitle[..50];
            }
            
            var fileName = $"video_{timestamp}_{safeTitle}.mp4";
            var filePath = Path.Combine(outputDir, fileName);

            // Save video file
            await File.WriteAllBytesAsync(filePath, downloadResponse.Data!);
            
            var fileSize = downloadResponse.Data!.Length / 1024.0 / 1024.0; // MB
            System.Console.WriteLine($"💾 Video saved: {filePath}");
            System.Console.WriteLine($"📏 File size: {fileSize:F2} MB");

            return new ApiResponse<string>
            {
                IsSuccess = true,
                Data = filePath,
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Error saving video: {ex.Message}",
                StatusCode = 500
            };
        }
    }
}
