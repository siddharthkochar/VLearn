namespace VLearn.Console.Models;

/// <summary>
/// Represents responses from API calls
/// </summary>
public record ApiResponse<T>(
    bool IsSuccess,
    T? Data,
    string ErrorMessage,
    int StatusCode
);
