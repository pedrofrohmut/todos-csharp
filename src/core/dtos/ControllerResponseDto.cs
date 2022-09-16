namespace Todos.Core.Dtos;

public class ControllerResponseDto
{
    public object? Body { get; init; }
    public string Msg { get; init; } = "";
    public int Status { get; init; }
}
