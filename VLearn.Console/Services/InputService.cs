using VLearn.Console.Models;
using VLearn.Console.Extensions;

namespace VLearn.Console.Services;

public interface IInputService
{
    Task<InputText> GetInputAsync(string? filePath = null);
    ScriptGenerationRequest GetScriptGenerationRequest(string inputText);
}

public class InputService : IInputService
{
    public async Task<InputText> GetInputAsync(string? filePath = null)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            // Read from file
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var content = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("File is empty or contains only whitespace.");
            }

            return new InputText
            {
                Content = content.Trim(),
                Source = filePath
            };
        }
        else
        {
            WriteLine("Enter your text content (press Enter twice to finish):");
            WriteLine("=".PadRight(50, '='));
            
            var lines = new List<string>();
            string? line;
            int emptyLineCount = 0;

            while ((line = ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2)
                    {
                        break;
                    }
                }
                else
                {
                    emptyLineCount = 0;
                }
                lines.Add(line);
            }

            var content = string.Join(Environment.NewLine, lines).Trim();
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("No content provided.");
            }

            return new InputText
            {
                Content = content,
                Source = "console"
            };
        }
    }

    public ScriptGenerationRequest GetScriptGenerationRequest(string inputText)
    {
        WriteLine();
        WriteLine("📝 Script Generation Options");
        WriteLine("=".PadRight(50, '='));
        
        // Get script type
        var scriptType = GetScriptTypeFromUser();
        
        // Get duration
        var duration = GetDurationFromUser();
        
        // Get optional custom instructions
        var customInstructions = GetCustomInstructionsFromUser();

        return new ScriptGenerationRequest
        {
            InputText = inputText,
            ScriptType = scriptType,
            DurationSeconds = duration,
            CustomInstructions = customInstructions
        };
    }

    private static ScriptType GetScriptTypeFromUser()
    {
        WriteLine();
        WriteLine("🎬 Choose script type:");
        
        for (int i = 1; i <= 7; i++)
        {
            var scriptType = (ScriptType)i;
            WriteLine($"{i}. {scriptType.GetDisplayName()} - {scriptType.GetDescription()}");
        }
        WriteLine();

        while (true)
        {
            Write("Enter your choice (1-7): ");
            var input = ReadLine();
            
            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= 7)
            {
                var selectedType = (ScriptType)choice;
                WriteLine($"✅ Selected: {selectedType.GetDisplayName()}");
                return selectedType;
            }
            
            WriteLine("❌ Invalid choice. Please enter a number between 1 and 7.");
        }
    }

    private static int GetDurationFromUser()
    {
        WriteLine();
        WriteLine("⏱️ Video duration options:");
        WriteLine("1. Very Short (10-30 seconds) - Quick concept overview");
        WriteLine("2. Short (30 seconds - 2 minutes) - Brief explanation");
        WriteLine("3. Medium (2-5 minutes) - Detailed explanation");
        WriteLine("4. Long (5-10 minutes) - Comprehensive coverage");
        WriteLine("5. Custom duration in seconds");
        WriteLine("Pro Tip I: Choose custom 10s for quick generation");
        WriteLine("Pro Tip II: Remember! longer the video, longer the processing time");
        WriteLine();

        while (true)
        {
            Write("Enter your choice (1-5): ");
            var input = ReadLine();
            
            switch (input)
            {
                case "1":
                    return GetDurationInRange(10, 30, "very short");
                case "2":
                    return GetDurationInRange(30, 120, "short");
                case "3":
                    return GetDurationInRange(120, 300, "medium");
                case "4":
                    return GetDurationInRange(300, 600, "long");
                case "5":
                    return GetCustomDurationInSeconds();
                default:
                    WriteLine("❌ Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }
    }

    private static int GetDurationInRange(int minSeconds, int maxSeconds, string rangeName)
    {
        Write($"Enter duration in seconds ({minSeconds}-{maxSeconds}s for {rangeName} video): ");
        while (true)
        {
            var input = ReadLine();
            if (int.TryParse(input, out var duration) && duration >= minSeconds && duration <= maxSeconds)
            {
                WriteLine($"✅ Selected: {duration} seconds ({duration / 60.0:F1} minutes)");
                return duration;
            }
            WriteLine($"❌ Please enter a number between {minSeconds} and {maxSeconds} seconds.");
            Write($"Enter duration in seconds ({minSeconds}-{maxSeconds}): ");
        }
    }

    private static int GetCustomDurationInSeconds()
    {
        Write("Enter custom duration in seconds (10-1800s / 10s-30min): ");
        while (true)
        {
            var input = ReadLine();
            if (int.TryParse(input, out var duration) && duration >= 10 && duration <= 1800)
            {
                WriteLine($"✅ Selected: {duration} seconds ({duration / 60.0:F1} minutes)");
                return duration;
            }
            WriteLine("❌ Please enter a number between 10 and 1800 seconds (10s to 30 minutes).");
            Write("Enter duration in seconds: ");
        }
    }

    private static string? GetCustomInstructionsFromUser()
    {
        WriteLine();
        WriteLine("📋 Custom Instructions (optional):");
        WriteLine("Add any specific requirements or preferences for your script.");
        WriteLine("Press Enter to skip, or type your instructions:");
        
        var instructions = ReadLine();
        return string.IsNullOrWhiteSpace(instructions) ? null : instructions.Trim();
    }
}
