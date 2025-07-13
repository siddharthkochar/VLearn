namespace VLearn.Console.Models;

/// <summary>
/// Represents different types of learning video scripts
/// </summary>
public enum ScriptType
{
    Standard = 1,
    Storytelling = 2,
    Documentary = 3,
    Tutorial = 4,
    Explainer = 5,
    CaseStudy = 6,
    Conversational = 7
}

/// <summary>
/// Represents the generated script from Gemini API
/// </summary>
public class Script
{
    public string Content { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public int EstimatedDurationSeconds { get; set; }
    public ScriptType Type { get; set; } = ScriptType.Standard;
    public int RequestedDurationSeconds { get; set; }
    
    /// <summary>
    /// Helper property to get requested duration in minutes for display purposes
    /// </summary>
    public double RequestedDurationMinutes => RequestedDurationSeconds / 60.0;
    
    /// <summary>
    /// Helper property to get estimated duration in minutes for display purposes
    /// </summary>
    public double EstimatedDurationMinutes => EstimatedDurationSeconds / 60.0;
}
