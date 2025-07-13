using System.Text.Json.Serialization;

namespace VLearn.Console.Models;

/// <summary>
/// Represents HeyGen video creation request
/// </summary>
public record HeyGenVideoRequest(
    [property: JsonPropertyName("video_inputs")] List<HeyGenVideoInput> VideoInputs,
    [property: JsonPropertyName("dimension")] HeyGenDimension? Dimension = null
);

public record HeyGenVideoInput(
    [property: JsonPropertyName("character")] HeyGenCharacter Character,
    [property: JsonPropertyName("voice")] HeyGenVoice Voice
);

public record HeyGenCharacter(
    [property: JsonPropertyName("type")] string Type = "avatar",
    [property: JsonPropertyName("avatar_id")] string AvatarId = "Abigail_expressive_2024112501",
    [property: JsonPropertyName("avatar_style")] string AvatarStyle = "normal"
);

public record HeyGenVoice(
    [property: JsonPropertyName("input_text")] string InputText,
    [property: JsonPropertyName("type")] string Type = "text",
    [property: JsonPropertyName("voice_id")] string VoiceId = "73c0b6a2e29d4d38aca41454bf58c955",
    [property: JsonPropertyName("speed")] double Speed = 1.1
);

public record HeyGenBackground(
    [property: JsonPropertyName("type")] string Type = "color",
    [property: JsonPropertyName("value")] string Value = "#008000"
);

public record HeyGenDimension(
    [property: JsonPropertyName("width")] int Width = 1280,
    [property: JsonPropertyName("height")] int Height = 720
);

/// <summary>
/// Represents HeyGen video creation response
/// </summary>
public record HeyGenVideoResponse(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("data")] HeyGenVideoData Data,
    [property: JsonPropertyName("message")] string Message
);

public record HeyGenVideoData(
    [property: JsonPropertyName("video_id")] string VideoId
);

/// <summary>
/// Represents HeyGen video status response
/// </summary>
public record HeyGenVideoStatusResponse(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("data")] HeyGenVideoStatusData Data,
    [property: JsonPropertyName("message")] string Message
);

public record HeyGenVideoStatusData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("callback_id")] string? CallbackId = null,
    [property: JsonPropertyName("caption_url")] string? CaptionUrl = null,
    [property: JsonPropertyName("created_at")] long CreatedAt = 0,
    [property: JsonPropertyName("duration")] double Duration = 0,
    [property: JsonPropertyName("error")] string? Error = null,
    [property: JsonPropertyName("gif_url")] string? GifUrl = null,
    [property: JsonPropertyName("thumbnail_url")] string? ThumbnailUrl = null,
    [property: JsonPropertyName("video_url")] string? VideoUrl = null,
    [property: JsonPropertyName("video_url_caption")] string? VideoUrlCaption = null
);
