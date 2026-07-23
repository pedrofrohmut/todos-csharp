using Todos.Core.Errors;

namespace Todos.Core.Utils;

internal class NoErrorOnSuccessException : Exception
{
    internal NoErrorOnSuccessException():
        base("Result won't have a ResultError object in case of success.") {}
}

internal class NoPayloadOnFailureException : Exception
{
    internal NoPayloadOnFailureException():
        base("Result won't have a Payload object in case of failure.") {}
}

public class Result
{
    private readonly bool isSuccess;
    private readonly Option<ResultError> error;

    public bool IsSuccess { get => this.isSuccess; }

    public ResultError Error
    {
        get {
            if (!IsSuccess || this.error.HasNone) {
                throw new NoErrorOnSuccessException();
            }
            return error.Value;
        }
    }

    private Result(bool isSuccess, ResultError? error)
    {
        this.isSuccess = isSuccess;
        this.error = Option.New(error);
    }

    public static Result Ok()
    {
        return new Result(isSuccess: true, error: null);
    }

    public static Result Fail(ResultError? error)
    {
        return new Result(isSuccess: false, error: error);
    }

    public static Result Fail(string code, string message)
    {
        var error = new ResultError(code, message);
        return new Result(isSuccess: false, error: error);
    }
}

public class Result<T>
{
    private readonly bool isSuccess;
    private readonly ResultError? error;
    private readonly T? payload;

    public bool IsSuccess { get => this.isSuccess; }

    public ResultError Error
    {
        get {
            if (IsSuccess || this.error is null) {
                throw new NoErrorOnSuccessException();
            }
            return this.error;
        }
    }

    public T Payload
    {
        get {
            if (!IsSuccess || this.payload is null) {
                throw new NoPayloadOnFailureException();
            }
            return this.payload;
        }
    }

    private Result(bool isSuccess, ResultError? error, T? payload)
    {
        this.isSuccess = isSuccess;
        this.error = error;
        this.payload = payload;
    }

    public static Result<T> Ok()
    {
        return new Result<T>(isSuccess: true, error: null, payload: default);
    }

    public static Result<T> Ok(T payload)
    {
        return new Result<T>(isSuccess: true, error: null, payload: payload);
    }

    public static Result<T> Fail(ResultError error)
    {
        return new Result<T>(isSuccess: false, error: error, payload: default);
    }

    public static Result<T> Fail(string code, string message)
    {
        var error = new ResultError(code, message);
        return new Result<T>(isSuccess: false, error: error, payload: default);
    }
}
