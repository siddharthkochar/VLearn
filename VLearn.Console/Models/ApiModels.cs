namespace VLearn.Console.Models;

/// <summary>
/// Represents responses from API calls
/// </summary>
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}

/// <summary>
/// Represents Synthesia video creation response
/// </summary>
public class VideoResponse
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? DownloadUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents Gemini API response
/// </summary>
public class GeminiResponse
{
    public List<GeminiCandidate> Candidates { get; set; } = new();
}

public class GeminiCandidate
{
    public GeminiContent Content { get; set; } = new();
}

public class GeminiContent
{
    public List<GeminiPart> Parts { get; set; } = new();
}

public class GeminiPart
{
    public string Text { get; set; } = string.Empty;
}
