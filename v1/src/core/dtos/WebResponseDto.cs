namespace Todos.Core.Dtos;

public class WebResponseDto
{
    public object? Body { get; init; } = null;
    public string Message { get; init; } = "";
    public int Status { get; init; }

    public object? Value {
        get { return Body == null ? Message : Body; }
    }
}
