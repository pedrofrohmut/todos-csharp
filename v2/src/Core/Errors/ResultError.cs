namespace Todos.Core.Errors;

public class ResultError
{
    public Enum Code { get; }
    public string Message { get; }

    public ResultError(Enum code, string message)
    {
        Code = code;
        Message = message;
    }
}
