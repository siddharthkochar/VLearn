namespace VLearn.Console.Models;

/// <summary>
/// Represents the input text from user (console or file)
/// </summary>
public class InputText
{
    public string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty; // "console" or file path
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
