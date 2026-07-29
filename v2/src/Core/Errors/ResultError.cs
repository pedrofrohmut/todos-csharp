namespace Todos.Core.Errors;

// TODO: Make the Enum not optional. And fix all the compiler errors
// Maybe String Code is no longer needed. Can be only the enum + message

public class ResultError
{
    public string Code { get; init; }
    public string Message { get; init; }
    public Enum? EnumCode { get; init; }

    public ResultError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public ResultError(Enum enumCode, string message)
    {
        Code = enumCode.ToString();
        EnumCode = enumCode;
        Message = message;
    }
}
