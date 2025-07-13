using VLearn.Console.Models;
using VLearn.Console.Services;

namespace VLearn.Console.Services;

/// <summary>
/// Video processing service using HeyGen
/// </summary>
public interface IVideoProcessingService
{
    Task<ApiResponse<string>> ProcessVideoAsync(Script script);
}

public class VideoProcessingService : IVideoProcessingService
{
    private readonly IHeyGenService _heyGenService;
    private readonly int _maxPollingAttempts = 60; // 5 minutes with 5-second intervals
    private readonly int _pollingIntervalSeconds = 5;

    public VideoProcessingService(IHeyGenService heyGenService)
    {
        _heyGenService = heyGenService;
    }

    public async Task<ApiResponse<string>> ProcessVideoAsync(Script script)
    {
        try
        {
            System.Console.WriteLine("🎬 Creating video with HeyGen...");

            // Step 1: Create video
            var createResponse = await _heyGenService.CreateVideoAsync(script);
            
            if (!createResponse.IsSuccess)
            {
                return new ApiResponse<string>(
                    IsSuccess: false,
                    Data: null,
                    ErrorMessage: $"Failed to create video with HeyGen: {createResponse.ErrorMessage}",
                    StatusCode: createResponse.StatusCode
                );
            }

            var videoId = createResponse.Data!.Data.VideoId;
            System.Console.WriteLine($"✅ HeyGen video creation started. Video ID: {videoId}");
            System.Console.WriteLine("⏰ Video is being processed. This typically takes 3-5 minutes...");

            // Step 2: Poll for completion
            var pollResult = await PollForHeyGenCompletion(videoId);
            if (!pollResult.IsSuccess)
            {
                return new ApiResponse<string>(
                    IsSuccess: false,
                    Data: null,
                    ErrorMessage: pollResult.ErrorMessage,
                    StatusCode: pollResult.StatusCode
                );
            }

            var completedVideo = pollResult.Data!;
            
            // Step 3: Download video
            if (string.IsNullOrEmpty(completedVideo.Data.VideoUrl))
            {
                return new ApiResponse<string>(
                    IsSuccess: false,
                    Data: null,
                    ErrorMessage: "HeyGen video completed but no download URL provided",
                    StatusCode: 500
                );
            }

            var downloadResult = await DownloadAndSaveVideo(completedVideo.Data.VideoUrl, script.Title);
            return downloadResult;
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(
                IsSuccess: false,
                Data: null,
                ErrorMessage: $"Error in video processing: {ex.Message}",
                StatusCode: 500
            );
        }
    }

    private async Task<ApiResponse<HeyGenVideoStatusResponse>> PollForHeyGenCompletion(string videoId)
    {
        for (int attempt = 1; attempt <= _maxPollingAttempts; attempt++)
        {
            System.Console.WriteLine($"🔄 Checking HeyGen video status... (Attempt {attempt}/{_maxPollingAttempts})");
            
            var statusResponse = await _heyGenService.GetVideoStatusAsync(videoId);
            
            if (!statusResponse.IsSuccess)
            {
                return new ApiResponse<HeyGenVideoStatusResponse>(
                    IsSuccess: false,
                    Data: null,
                    ErrorMessage: $"Failed to check video status: {statusResponse.ErrorMessage}",
                    StatusCode: statusResponse.StatusCode
                );
            }

            var video = statusResponse.Data!;
            System.Console.WriteLine($"📊 Video status: {video.Data.Status}");

            switch (video.Data.Status.ToLower())
            {
                case "complete":
                case "completed":
                    System.Console.WriteLine("✅ HeyGen video processing completed!");
                    return statusResponse;
                
                case "failed":
                case "error":
                    return new ApiResponse<HeyGenVideoStatusResponse>(
                        IsSuccess: false,
                        Data: null,
                        ErrorMessage: "Video processing failed on HeyGen platform",
                        StatusCode: 500
                    );
                
                case "processing":
                case "pending":
                    // Continue polling
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
                
                default:
                    System.Console.WriteLine($"⚠️ Unknown status: {video.Data.Status}. Continuing to poll...");
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
            }
        }

        return new ApiResponse<HeyGenVideoStatusResponse>(
            IsSuccess: false,
            Data: null,
            ErrorMessage: $"HeyGen video processing timed out after {_maxPollingAttempts * _pollingIntervalSeconds / 60} minutes",
            StatusCode: 408
        );
    }

    private async Task<ApiResponse<string>> DownloadAndSaveVideo(string downloadUrl, string title)
    {
        try
        {
            // Download video using HeyGen service
            var downloadResponse = await _heyGenService.DownloadVideoAsync(downloadUrl);
            
            if (!downloadResponse.IsSuccess)
            {
                return new ApiResponse<string>(
                    IsSuccess: false,
                    Data: null,
                    ErrorMessage: downloadResponse.ErrorMessage,
                    StatusCode: downloadResponse.StatusCode
                );
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
            
            var fileName = $"heygen_video_{timestamp}_{safeTitle}.mp4";
            var filePath = Path.Combine(outputDir, fileName);

            // Save video file
            await File.WriteAllBytesAsync(filePath, downloadResponse.Data!);
            
            var fileSize = downloadResponse.Data!.Length / 1024.0 / 1024.0; // MB
            System.Console.WriteLine($"💾 HeyGen video saved: {filePath}");
            System.Console.WriteLine($"📏 File size: {fileSize:F2} MB");

            return new ApiResponse<string>(
                IsSuccess: true,
                Data: filePath,
                ErrorMessage: string.Empty,
                StatusCode: 200
            );
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(
                IsSuccess: false,
                Data: null,
                ErrorMessage: $"Error saving HeyGen video: {ex.Message}",
                StatusCode: 500
            );
        }
    }
}
