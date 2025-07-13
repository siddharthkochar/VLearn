using VLearn.Console.Models;

namespace VLearn.Console.Extensions;

public static class ScriptTypeExtensions
{
    public static string GetDisplayName(this ScriptType scriptType)
    {
        return scriptType switch
        {
            ScriptType.Standard => "Standard Educational",
            ScriptType.Storytelling => "Storytelling Narrative",
            ScriptType.Documentary => "Documentary Style",
            ScriptType.Tutorial => "Step-by-Step Tutorial",
            ScriptType.Explainer => "Simplified Explainer",
            ScriptType.CaseStudy => "Real-World Case Study",
            ScriptType.Conversational => "Conversational Style",
            _ => "Unknown"
        };
    }

    public static string GetDescription(this ScriptType scriptType)
    {
        return scriptType switch
        {
            ScriptType.Standard => "Clear, educational format with structured content",
            ScriptType.Storytelling => "Engaging narrative with characters and scenarios",
            ScriptType.Documentary => "Professional, authoritative tone with facts",
            ScriptType.Tutorial => "Step-by-step instructional format",
            ScriptType.Explainer => "Simplified explanations with analogies",
            ScriptType.CaseStudy => "Real-world examples and practical applications",
            ScriptType.Conversational => "Natural dialogue style like talking to a friend",
            _ => "Unknown script type"
        };
    }
}
