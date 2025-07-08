namespace VLearn.Console.Models;

/// <summary>
/// Represents a video generation request for Synthesia API
/// </summary>
public class VideoRequest
{
    public string ScriptText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Avatar { get; set; } = "anna_costume1_cameraA"; // Default avatar
    public string Background { get; set; } = "green_screen"; // Default background
    public bool Test { get; set; } = true; // For testing purposes
}
