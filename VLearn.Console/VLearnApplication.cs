using VLearn.Console.Services;
using VLearn.Console.Extensions;

namespace VLearn.Console;

public class VLearnApplication
{
    private readonly IInputService _inputService;
    private readonly IGeminiService _geminiService;
    private readonly IVideoProcessingService _videoProcessingService;

    public VLearnApplication(IInputService inputService, IGeminiService geminiService, IVideoProcessingService videoProcessingService)
    {
        _inputService = inputService;
        _geminiService = geminiService;
        _videoProcessingService = videoProcessingService;
    }

    public async Task RunAsync(string? filePath = null)
    {
        // Step 1: Get input
        WriteLine("📖 Processing input...");
        
        var input = await _inputService.GetInputAsync(filePath);

        WriteLine($"✅ Input received from: {input.Source}");
        WriteLine($"📝 Content length: {input.Content.Length} characters");
        WriteLine();

        // Step 2: Get script generation preferences
        var scriptRequest = _inputService.GetScriptGenerationRequest(input.Content);

        // Step 3: Generate script with Gemini
        WriteLine("🧠 Generating script with Gemini...");
        var scriptResponse = await _geminiService.GenerateScriptAsync(scriptRequest);
            
        if (!scriptResponse.IsSuccess)
        {
            WriteLine($"❌ Script generation failed: {scriptResponse.ErrorMessage}");
            return;
        }

        var script = scriptResponse.Data!;
        WriteLine($"✅ Script generated successfully!");
        WriteLine($"📝 Script type: {script.Type.GetDisplayName()}");
        WriteLine($"📝 Script title: {script.Title}");
        WriteLine($"📝 Requested duration: {script.RequestedDurationSeconds} seconds ({script.RequestedDurationMinutes:F1} minutes)");
        WriteLine($"⏱️ Estimated actual duration: {script.EstimatedDurationSeconds} seconds ({script.EstimatedDurationMinutes:F1} minutes)");
        WriteLine();

        // Step 4: Create video with HeyGen
        WriteLine("🎥 Creating video with HeyGen...");
        var videoResult = await _videoProcessingService.ProcessVideoAsync(script);
            
        if (!videoResult.IsSuccess)
        {
            WriteLine($"❌ Video creation failed: {videoResult.ErrorMessage}");
            return;
        }

        WriteLine($"🎉 Video ready: {videoResult.Data}");
        WriteLine();
            
        // Ask user if they want to view the video
        await PromptToViewVideo(videoResult.Data!);

        // Show script preview for reference
        WriteLine("📋 Generated Script Preview:");
        WriteLine("=".PadRight(50, '='));
        WriteLine(script.Content.Length > 300 
            ? script.Content[..300] + "..." 
            : script.Content);
        WriteLine("=".PadRight(50, '='));
    }

    private async Task PromptToViewVideo(string videoFilePath)
    {
        WriteLine();
        Write("🎬 Would you like to view the generated video? (y/N): ");
        
        var response = ReadLine()?.Trim().ToLower();
        
        if (response == "y" || response == "yes")
        {
            WriteLine("🚀 Opening video...");
            await PlayVideo(videoFilePath);
            return;
        }

        WriteLine($"📁 Video saved at: {videoFilePath}");
    }

    private async Task PlayVideo(string videoFilePath)
    {
        if (!File.Exists(videoFilePath))
        {
            throw new FileNotFoundException($"Video file not found: {videoFilePath}");
        }

        System.Diagnostics.ProcessStartInfo startInfo;

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            // Windows - use default video player
            startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = videoFilePath,
                UseShellExecute = true
            };
        }
        else if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            // Linux/macOS - try common video players
            var players = new[] { "vlc", "mpv", "mplayer", "open" }; // open is for macOS
                
            string? availablePlayer = null;
            foreach (var player in players)
            {
                try
                {
                    var which = await RunCommand("which", player);
                    if (!string.IsNullOrEmpty(which))
                    {
                        availablePlayer = player;
                        break;
                    }
                }
                catch
                {
                    // Player not found, continue to next
                }
            }

            if (availablePlayer == null)
            {
                throw new InvalidOperationException("No suitable video player found. Please install VLC, MPV, or MPlayer.");
            }

            startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = availablePlayer,
                Arguments = $"\"{videoFilePath}\"",
                UseShellExecute = false
            };
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system");
        }

        using var process = System.Diagnostics.Process.Start(startInfo);
        WriteLine("✅ Video player launched successfully!");
    }

    private async Task<string> RunCommand(string command, string arguments)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
        {
            return string.Empty;
        }

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        
        return process.ExitCode == 0 ? output.Trim() : string.Empty;
    }
}
