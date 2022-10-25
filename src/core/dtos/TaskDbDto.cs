namespace Todos.Core.Dtos;

public class TaskDbDto
{
    public Guid   Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public Guid   UserId { get; init; } = Guid.Empty;

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Description: {Description}, UserId: {UserId}";
}
