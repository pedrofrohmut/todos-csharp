namespace Todos.Core.Errors;

public class ResultError
{
    public string Code { get; init; }
    public string Message { get; init; }

    public ResultError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
