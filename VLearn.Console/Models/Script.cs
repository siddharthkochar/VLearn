namespace VLearn.Console.Models;

/// <summary>
/// Represents the generated script from Gemini API
/// </summary>
public class Script
{
    public string Content { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public int EstimatedDurationSeconds { get; set; }
}
