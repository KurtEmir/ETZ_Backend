namespace ETZ.Application;

public class Response<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static Response<T> Ok(T? data, string? message = null) => new() { Success = true, Data = data, Message = message };
    public static Response<T> Fail(string message) => new() { Success = false, Message = message };
}

public class Response
{
    public bool Success { get; init; }
    public string? Message { get; init; }

    public static Response Ok(string? message = null) => new() { Success = true, Message = message };
    public static Response Fail(string message) => new() { Success = false, Message = message };
}