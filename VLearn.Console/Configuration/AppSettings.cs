namespace VLearn.Console.Configuration;

/// <summary>
/// Configuration settings for the application
/// </summary>
public class AppSettings
{
    public GeminiApiSettings GeminiApi { get; set; } = new();
    public HeyGenApiSettings HeyGenApi { get; set; } = new();
}

public class GeminiApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}

public class HeyGenApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
