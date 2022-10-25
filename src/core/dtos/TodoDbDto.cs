namespace Todos.Core.Dtos;

public class TodoDbDto
{
    public Guid   Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public bool   IsDone { get; init; } = false;
    public Guid   TaskId { get; init; } = Guid.Empty;
    public Guid   UserId { get; init; } = Guid.Empty;
}
