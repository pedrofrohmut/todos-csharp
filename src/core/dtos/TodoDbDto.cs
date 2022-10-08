namespace Todos.Core.Dtos;

public class TodoDbDto
{
    public string Id          { get; init; } = "";
    public string Name        { get; init; } = "";
    public string Description { get; init; } = "";
    public bool   IsDone      { get; init; } = false;
    public string TaskId      { get; init; } = "";
    public string UserId      { get; init; } = "";
}
