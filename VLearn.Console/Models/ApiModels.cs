using System.Text.Json.Serialization;

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
/// Represents HeyGen video creation request
/// </summary>
public class HeyGenVideoRequest
{
    [JsonPropertyName("video_inputs")]
    public List<HeyGenVideoInput> VideoInputs { get; set; } = new();
    
    [JsonPropertyName("dimension")]
    public HeyGenDimension? Dimension { get; set; }
}

public class HeyGenVideoInput
{
    [JsonPropertyName("character")]
    public HeyGenCharacter Character { get; set; } = new();
    
    [JsonPropertyName("voice")]
    public HeyGenVoice Voice { get; set; } = new();
}

public class HeyGenCharacter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "avatar";
    
    [JsonPropertyName("avatar_id")]
    public string AvatarId { get; set; } = "Abigail_expressive_2024112501";
    
    [JsonPropertyName("avatar_style")]
    public string AvatarStyle { get; set; } = "normal";
}

public class HeyGenVoice
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";
    
    [JsonPropertyName("input_text")]
    public string InputText { get; set; } = string.Empty;
    
    [JsonPropertyName("voice_id")]
    public string VoiceId { get; set; } = "73c0b6a2e29d4d38aca41454bf58c955";
    
    [JsonPropertyName("speed")]
    public double Speed { get; set; } = 1.1;
}

public class HeyGenBackground
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "color";
    
    [JsonPropertyName("value")]
    public string Value { get; set; } = "#008000";
}

public class HeyGenDimension
{
    [JsonPropertyName("width")]
    public int Width { get; set; } = 1280;
    
    [JsonPropertyName("height")]
    public int Height { get; set; } = 720;
}

/// <summary>
/// Represents HeyGen video creation response
/// </summary>
public class HeyGenVideoResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("data")]
    public HeyGenVideoData Data { get; set; } = new();
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}

public class HeyGenVideoData
{
    [JsonPropertyName("video_id")]
    public string VideoId { get; set; } = string.Empty;
}

/// <summary>
/// Represents HeyGen video status response
/// </summary>
public class HeyGenVideoStatusResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("data")]
    public HeyGenVideoStatusData Data { get; set; } = new();
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}

public class HeyGenVideoStatusData
{
    [JsonPropertyName("callback_id")]
    public string? CallbackId { get; set; }
    
    [JsonPropertyName("caption_url")]
    public string? CaptionUrl { get; set; }
    
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }
    
    [JsonPropertyName("duration")]
    public double Duration { get; set; }
    
    [JsonPropertyName("error")]
    public string? Error { get; set; }
    
    [JsonPropertyName("gif_url")]
    public string? GifUrl { get; set; }
    
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }
    
    [JsonPropertyName("video_url")]
    public string? VideoUrl { get; set; }
    
    [JsonPropertyName("video_url_caption")]
    public string? VideoUrlCaption { get; set; }
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
