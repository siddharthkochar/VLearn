namespace VLearn.Console.Models;

public record ApiResponse<T>(
    bool IsSuccess,
    T? Data,
    string ErrorMessage,
    int StatusCode
);
