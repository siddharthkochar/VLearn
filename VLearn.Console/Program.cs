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

    public VLearnApplication(IInputService inputService)
    {
        _inputService = inputService;
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

            // TODO: Phase 2 - Gemini integration will go here
            System.Console.WriteLine("🧠 Generating script with Gemini... (Phase 2 - Not implemented yet)");
            
            // TODO: Phase 3 - Synthesia integration will go here  
            System.Console.WriteLine("🎬 Creating video with Synthesia... (Phase 3 - Not implemented yet)");
            
            // For now, just show what we received
            System.Console.WriteLine();
            System.Console.WriteLine("📋 Input Preview:");
            System.Console.WriteLine("-".PadRight(30, '-'));
            System.Console.WriteLine(input.Content.Length > 200 
                ? input.Content[..200] + "..." 
                : input.Content);
            System.Console.WriteLine("-".PadRight(30, '-'));
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error processing input: {ex.Message}");
            throw;
        }
    }
}
