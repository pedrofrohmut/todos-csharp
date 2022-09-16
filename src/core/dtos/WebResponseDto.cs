namespace Todos.Core.Dtos;

public class WebResponseDto
{
    public object? Body { get; init; }
    public string Message { get; init; } = "";
    public int Status { get; init; }
}
