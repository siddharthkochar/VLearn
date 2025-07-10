using VLearn.Console.Models;
using VLearn.Console.Services;

namespace VLearn.Console.Services;

/// <summary>
/// Enhanced video processing service with dual provider support
/// </summary>
public interface IDualVideoProcessingService
{
    Task<ApiResponse<string>> ProcessVideoAsync(Script script, VideoProvider preferredProvider = VideoProvider.DeepBrainAI);
}

/// <summary>
/// Video provider enumeration
/// </summary>
public enum VideoProvider
{
    Synthesia,
    DeepBrainAI
}

public class DualVideoProcessingService : IDualVideoProcessingService
{
    private readonly ISynthesiaService _synthesiaService;
    private readonly IDeepBrainService _deepBrainService;
    private readonly int _maxPollingAttempts = 60; // 5 minutes with 5-second intervals
    private readonly int _pollingIntervalSeconds = 5;

    public DualVideoProcessingService(ISynthesiaService synthesiaService, IDeepBrainService deepBrainService)
    {
        _synthesiaService = synthesiaService;
        _deepBrainService = deepBrainService;
    }

    public async Task<ApiResponse<string>> ProcessVideoAsync(Script script, VideoProvider preferredProvider = VideoProvider.DeepBrainAI)
    {
        try
        {
            System.Console.WriteLine($"🎬 Attempting video creation with {preferredProvider}...");

            // Try primary provider first
            var primaryResult = await TryCreateVideoWithProvider(script, preferredProvider);
            if (primaryResult.IsSuccess)
            {
                return primaryResult;
            }

            // If primary fails, try fallback provider
            var fallbackProvider = preferredProvider == VideoProvider.DeepBrainAI ? VideoProvider.Synthesia : VideoProvider.DeepBrainAI;
            System.Console.WriteLine($"⚠️ {preferredProvider} failed: {primaryResult.ErrorMessage}");
            System.Console.WriteLine($"🔄 Falling back to {fallbackProvider}...");

            var fallbackResult = await TryCreateVideoWithProvider(script, fallbackProvider);
            if (fallbackResult.IsSuccess)
            {
                return fallbackResult;
            }

            // Both providers failed
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Both video providers failed. {preferredProvider}: {primaryResult.ErrorMessage}. {fallbackProvider}: {fallbackResult.ErrorMessage}",
                StatusCode = 500
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Error in dual video processing: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    private async Task<ApiResponse<string>> TryCreateVideoWithProvider(Script script, VideoProvider provider)
    {
        try
        {
            switch (provider)
            {
                case VideoProvider.DeepBrainAI:
                    return await ProcessWithDeepBrainAI(script);
                
                case VideoProvider.Synthesia:
                    return await ProcessWithSynthesia(script);
                
                default:
                    return new ApiResponse<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Unsupported video provider: {provider}",
                        StatusCode = 400
                    };
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Error with {provider}: {ex.Message}",
                StatusCode = 500
            };
        }
    }

    private async Task<ApiResponse<string>> ProcessWithDeepBrainAI(Script script)
    {
        // Step 1: Create video
        System.Console.WriteLine("🤖 Submitting DeepBrainAI video creation request...");
        var createResponse = await _deepBrainService.CreateVideoAsync(script);
        
        if (!createResponse.IsSuccess)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Failed to create video with DeepBrainAI: {createResponse.ErrorMessage}",
                StatusCode = createResponse.StatusCode
            };
        }

        var projectKey = createResponse.Data!.Key;
        System.Console.WriteLine($"✅ DeepBrainAI video creation started. Project Key: {projectKey}");
        System.Console.WriteLine("⏰ Video is being processed. This typically takes 3-5 minutes...");

        // Step 2: Poll for completion
        var pollResult = await PollForDeepBrainCompletion(projectKey);
        if (!pollResult.IsSuccess)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = pollResult.ErrorMessage,
                StatusCode = pollResult.StatusCode
            };
        }

        var completedProject = pollResult.Data!;
        
        // Step 3: Download video
        if (string.IsNullOrEmpty(completedProject.DownloadUrl))
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = "DeepBrainAI video completed but no download URL provided",
                StatusCode = 500
            };
        }

        var downloadResult = await DownloadAndSaveVideo(completedProject.DownloadUrl, script.Title, "deepbrain");
        return downloadResult;
    }

    private async Task<ApiResponse<string>> ProcessWithSynthesia(Script script)
    {
        // Step 1: Create video
        System.Console.WriteLine("🎬 Submitting Synthesia video creation request...");
        var createResponse = await _synthesiaService.CreateVideoAsync(script);
        
        if (!createResponse.IsSuccess)
        {
            return new ApiResponse<string>
            {
                IsSuccess = false,
                ErrorMessage = $"Failed to create video with Synthesia: {createResponse.ErrorMessage}",
                StatusCode = createResponse.StatusCode
            };
        }

        var videoId = createResponse.Data!.Id;
        System.Console.WriteLine($"✅ Synthesia video creation started. Video ID: {videoId}");
        System.Console.WriteLine("⏰ Video is being processed. This typically takes 3-5 minutes...");

        // Step 2: Poll for completion
        var pollResult = await PollForSynthesiaCompletion(videoId);
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
                ErrorMessage = "Synthesia video completed but no download URL provided",
                StatusCode = 500
            };
        }

        var downloadResult = await DownloadAndSaveVideo(completedVideo.DownloadUrl, script.Title, "synthesia");
        return downloadResult;
    }

    private async Task<ApiResponse<DeepBrainProjectResponse>> PollForDeepBrainCompletion(string projectKey)
    {
        for (int attempt = 1; attempt <= _maxPollingAttempts; attempt++)
        {
            System.Console.WriteLine($"🔄 Checking DeepBrainAI project status... (Attempt {attempt}/{_maxPollingAttempts})");
            
            var statusResponse = await _deepBrainService.GetProjectStatusAsync(projectKey);
            
            if (!statusResponse.IsSuccess)
            {
                return new ApiResponse<DeepBrainProjectResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to check project status: {statusResponse.ErrorMessage}",
                    StatusCode = statusResponse.StatusCode
                };
            }

            var project = statusResponse.Data!;
            System.Console.WriteLine($"📊 Project status: {project.Status}");

            switch (project.Status.ToLower())
            {
                case "complete":
                case "completed":
                    System.Console.WriteLine("✅ DeepBrainAI video processing completed!");
                    return statusResponse;
                
                case "failed":
                case "error":
                    return new ApiResponse<DeepBrainProjectResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Video processing failed on DeepBrainAI platform",
                        StatusCode = 500
                    };
                
                case "in_progress":
                case "progress":
                case "queued":
                case "pending":
                case "processing":
                    // Continue polling
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
                
                default:
                    System.Console.WriteLine($"⚠️ Unknown status: {project.Status}. Continuing to poll...");
                    if (attempt < _maxPollingAttempts)
                    {
                        await Task.Delay(_pollingIntervalSeconds * 1000);
                    }
                    break;
            }
        }

        return new ApiResponse<DeepBrainProjectResponse>
        {
            IsSuccess = false,
            ErrorMessage = $"DeepBrainAI video processing timed out after {_maxPollingAttempts * _pollingIntervalSeconds / 60} minutes",
            StatusCode = 408
        };
    }

    private async Task<ApiResponse<VideoResponse>> PollForSynthesiaCompletion(string videoId)
    {
        for (int attempt = 1; attempt <= _maxPollingAttempts; attempt++)
        {
            System.Console.WriteLine($"🔄 Checking Synthesia video status... (Attempt {attempt}/{_maxPollingAttempts})");
            
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
                    System.Console.WriteLine("✅ Synthesia video processing completed!");
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
            ErrorMessage = $"Synthesia video processing timed out after {_maxPollingAttempts * _pollingIntervalSeconds / 60} minutes",
            StatusCode = 408
        };
    }

    private async Task<ApiResponse<string>> DownloadAndSaveVideo(string downloadUrl, string title, string provider)
    {
        try
        {
            // Download video using appropriate service
            ApiResponse<byte[]> downloadResponse;
            
            if (provider == "deepbrain")
            {
                downloadResponse = await _deepBrainService.DownloadVideoAsync(downloadUrl);
            }
            else
            {
                downloadResponse = await _synthesiaService.DownloadVideoAsync(downloadUrl);
            }
            
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

            // Generate filename with provider prefix
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var safeTitle = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));
            if (safeTitle.Length > 50)
            {
                safeTitle = safeTitle[..50];
            }
            
            var fileName = $"{provider}_video_{timestamp}_{safeTitle}.mp4";
            var filePath = Path.Combine(outputDir, fileName);

            // Save video file
            await File.WriteAllBytesAsync(filePath, downloadResponse.Data!);
            
            var fileSize = downloadResponse.Data!.Length / 1024.0 / 1024.0; // MB
            System.Console.WriteLine($"💾 {provider} video saved: {filePath}");
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
                ErrorMessage = $"Error saving {provider} video: {ex.Message}",
                StatusCode = 500
            };
        }
    }
}
