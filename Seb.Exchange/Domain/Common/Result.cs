namespace Seb.Server.Domain.Common;

public class Result<T>
{
    public Result(T? value, string? message = null)
    {
        Value = value;
        Message = message;
    }
    
    public static Result<T> Error(string message) => new(default, message);
    public T? Value { get; }
    public string? Message { get; }

    public bool IsSuccess => Value is not null;
    public bool IsFailure => Value is null;

}