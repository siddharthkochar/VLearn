using System.Text.Json.Serialization;

namespace VLearn.Console.Models;

/// <summary>
/// Represents Gemini API response
/// </summary>
public record GeminiResponse(
    List<GeminiCandidate> Candidates
);

public record GeminiCandidate(
    GeminiContent Content
);

public record GeminiContent(
    List<GeminiPart> Parts
);

public record GeminiPart(
    string Text
);
