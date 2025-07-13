using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VLearn.Console;
using VLearn.Console.Configuration;
using VLearn.Console.Services;

WriteLine("🎬 VLearn V2 - AI Video Learning Console App");
WriteLine("=".PadRight(50, '='));

// Parse command line arguments
if (args.Length > 0 && args[0].ToLower() != "generate")
{
    WriteLine("Usage: vlearn generate [optional-file-path]");
    WriteLine("Examples:");
    WriteLine("  vlearn generate              # Interactive mode");
    WriteLine("  vlearn generate input.txt    # File input mode");
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

WriteLine("✅ Process completed successfully!");
return 0;

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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
