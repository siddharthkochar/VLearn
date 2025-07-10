namespace VLearn.Console.Configuration;

/// <summary>
/// Configuration settings for the application
/// </summary>
public class AppSettings
{
    public GeminiApiSettings GeminiApi { get; set; } = new();
    public SynthesiaApiSettings SynthesiaApi { get; set; } = new();
    public DeepBrainApiSettings DeepBrainApi { get; set; } = new();
}

public class GeminiApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}

public class SynthesiaApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}

public class DeepBrainApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
