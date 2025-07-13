namespace VLearn.Console.Models;

/// <summary>
/// Represents a request to generate a video script with specific parameters
/// </summary>
public class ScriptGenerationRequest
{
    public string InputText { get; set; } = string.Empty;
    public ScriptType ScriptType { get; set; } = ScriptType.Standard;
    public int DurationSeconds { get; set; } = 120; // Default 2 minutes in seconds
    public string? CustomInstructions { get; set; }
    
    /// <summary>
    /// Helper property to get duration in minutes for display purposes
    /// </summary>
    public double DurationMinutes => DurationSeconds / 60.0;
}