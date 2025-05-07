namespace API_BASE.Application.Common;

public static class ServiceResponseHelper
{
    // Resultados funcionales
    public static Result Success(string message = "Operación exitosa")
        => Result.Ok(message);

    public static Result Failure(string message)
        => Result.Fail(message);

    public static Result<T> Success<T>(T data, string? message = null)
        => Result<T>.Ok(data, message);

    public static Result<T> Failure<T>(string message)
        => Result<T>.Fail(message);

    // Respuestas de controlador
    public static ApiResponse<T> OkResponse<T>(T data, string? message = null)
        => ApiResponse<T>.Ok(data, message);

    public static ApiResponse<T> ErrorResponse<T>(string message)
        => ApiResponse<T>.Fail(message);

    public static ApiResponse<T> OkEmpty<T>(string? message = null)
        => ApiResponse<T>.Ok(message);
}
