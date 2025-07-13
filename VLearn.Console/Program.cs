using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VLearn.Console.Configuration;
using VLearn.Console.Services;

namespace VLearn.Console;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            System.Console.WriteLine("🎬 VLearn V2 - AI Video Learning Console App");
            System.Console.WriteLine("=".PadRight(50, '='));

            // Parse command line arguments
            if (args.Length > 0 && args[0].ToLower() != "generate")
            {
                System.Console.WriteLine("Usage: vlearn generate [optional-file-path]");
                System.Console.WriteLine("Examples:");
                System.Console.WriteLine("  vlearn generate              # Interactive mode");
                System.Console.WriteLine("  vlearn generate input.txt    # File input mode");
                return 1;
            }

            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup dependency injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get file path if provided
            string? filePath = args.Length > 1 ? args[1] : null;

            // Run the application
            var app = serviceProvider.GetRequiredService<VLearnApplication>();
            await app.RunAsync(filePath);

            System.Console.WriteLine("✅ Process completed successfully!");
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error: {ex.Message}");
            return 1;
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.Configure<AppSettings>(configuration);
        
        // Services
        services.AddSingleton<VLearnApplication>();
        services.AddScoped<IInputService, InputService>();
        services.AddScoped<IGeminiService, GeminiService>();
        services.AddScoped<IHeyGenService, HeyGenService>();
        services.AddScoped<IVideoProcessingService, VideoProcessingService>();
        
        // HTTP Client
        services.AddHttpClient();
    }
}

/// <summary>
/// Main application logic
/// </summary>
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
        System.Console.WriteLine("📖 Processing input...");
        
        try
        {
            var input = await _inputService.GetInputAsync(filePath);
            
            System.Console.WriteLine($"✅ Input received from: {input.Source}");
            System.Console.WriteLine($"📝 Content length: {input.Content.Length} characters");
            System.Console.WriteLine();

            // Step 2: Generate script with Gemini
            System.Console.WriteLine("🧠 Generating script with Gemini...");
            var scriptResponse = await _geminiService.GenerateScriptAsync(input.Content);
            
            if (!scriptResponse.IsSuccess)
            {
                System.Console.WriteLine($"❌ Script generation failed: {scriptResponse.ErrorMessage}");
                return;
            }

            var script = scriptResponse.Data!;
            System.Console.WriteLine($"✅ Script generated successfully!");
            System.Console.WriteLine($"📝 Script title: {script.Title}");
            System.Console.WriteLine($"📝 Script: {script.Content}");
            System.Console.WriteLine($"⏱️ Estimated duration: {script.EstimatedDurationSeconds} seconds");
            System.Console.WriteLine();

            // Step 3: Create video with HeyGen
            System.Console.WriteLine("🎥 Creating video with HeyGen...");
            var videoResult = await _videoProcessingService.ProcessVideoAsync(script);
            
            if (!videoResult.IsSuccess)
            {
                System.Console.WriteLine($"❌ Video creation failed: {videoResult.ErrorMessage}");
                return;
            }

            System.Console.WriteLine($"🎉 Video ready: {videoResult.Data}");
            System.Console.WriteLine();
            
            // Show script preview for reference
            System.Console.WriteLine("📋 Generated Script Preview:");
            System.Console.WriteLine("=".PadRight(50, '='));
            System.Console.WriteLine(script.Content.Length > 300 
                ? script.Content[..300] + "..." 
                : script.Content);
            System.Console.WriteLine("=".PadRight(50, '='));
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error processing input: {ex.Message}");
            throw;
        }
    }
}
