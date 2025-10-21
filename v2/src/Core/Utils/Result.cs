using Todos.Core.Errors;

namespace Todos.Core.Utils;

public class Result
{
    public bool IsSuccess { get; set; }
    public ResultError? Error { get; set; }

    protected Result() {}

    public static Result Successed()
    {
        return new Result { IsSuccess = true, Error = null };
    }

    public static Result Failed(ResultError error)
    {
        return new Result { IsSuccess = false, Error = error };
    }

    public static Result Failed(string code, string message)
    {
        var error = new ResultError(code, message);
        return new Result { IsSuccess = false, Error = error };
    }
}

public class Result<T> : Result
{
    public T? Payload { get; set; }

    private Result() {}

    public new static Result<T> Successed()
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = default };
    }

    public static Result<T> Successed(T payload)
    {
        return new Result<T> { IsSuccess = true, Error = null, Payload = payload };
    }

    public new static Result<T> Failed(ResultError error)
    {
        return new Result<T> { IsSuccess = false, Error = error, Payload = default };
    }

    public new static Result<T> Failed(string code, string message)
    {
        var error = new ResultError(code, message);
        return new Result<T> { IsSuccess = false, Error = error, Payload = default };
    }
}
