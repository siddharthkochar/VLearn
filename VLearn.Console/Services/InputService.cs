using VLearn.Console.Models;

namespace VLearn.Console.Services;

public interface IInputService
{
    Task<InputText> GetInputAsync(string? filePath = null);
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
}
