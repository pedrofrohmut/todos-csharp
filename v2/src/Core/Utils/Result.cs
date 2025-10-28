using Todos.Core.Errors;

namespace Todos.Core.Utils;

/*
    TODO: Add Exception property to the result so you can wrap exceptions with result
    also. This way you will only have try/catch in the lowest levels, in the high level
    you can just check for Result.Exception.

    In the controllers the try/catch should not be needed anymore.

    When in catch block, do not use Result.Failed anymore, use Result.Throw instead.

    This change should make the Result Errors and Exceptions two different and distinct
    things. Exceptions for errors that should not normally happen and Errors for the
    errors the scenarios the the developer want it to happen.

    TLDR: The developer expect the error is a Result.Error, every other error is a
    Result.Exception.
*/

public class Result
{
    public bool IsSuccess { get; set; }
    public ResultError? Error { get; set; }

    protected Result() { }

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

    private Result() { }

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
