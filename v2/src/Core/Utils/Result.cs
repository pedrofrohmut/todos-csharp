using Todos.Core.Errors;

namespace Todos.Core.Utils;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public ResultError? Error { get; set; }
    public object? Payload { get; set; }

    private Result() {}

    public static Result<T> Successed()
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = null };
    }

    public static Result<T> Successed(object payload)
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = payload };
    }

    public static Result<T> Failed(ResultError error)
    {
        return new Result<T> { IsSuccess = false, Error = error, Payload = null };
    }
}
