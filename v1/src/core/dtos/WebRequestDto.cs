namespace Todos.Core.Dtos;

public class WebRequestDto
{
    public object? Body { get; init; }
    public string? AuthUserId { get; init; }
    public string? Param { get; init; }
}
