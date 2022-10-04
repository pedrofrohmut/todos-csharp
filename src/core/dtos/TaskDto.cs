namespace Todos.Core.Dtos;

public class TaskDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string UserId { get; init; } = "";

    public override string ToString() => 
        $"Id: {Id}, Name: {Name}, Description: {Description}, UserId: {UserId}";
}
