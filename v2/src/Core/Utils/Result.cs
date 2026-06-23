using Todos.Core.Errors;

namespace Todos.Core.Utils;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public ResultError? Error { get; set; }
    public T? Payload { get; set; }

    private Result() {}

    public static Result<T> Ok()
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = default };
    }

    public static Result<T> Ok(T payload)
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = payload };
    }

    public static Result<T> Fail(ResultError? error)
    {
        if (error == null) {
            throw new ArgumentNullException();
        }
        return new Result<T> { IsSuccess = false, Error = error, Payload = default };
    }

    public static Result<T> Fail(string code, string message)
    {
        var error = new ResultError(code, message);
        return new Result<T> { IsSuccess = false, Error = error, Payload = default };
    }
}
