namespace Todos.Core.Dtos;

public class TodoDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public bool   IsDone { get; init; } = false;
    public string TaskId { get; init; } = "";
    public string UserId { get; init; } = "";

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Description: {Description}, IsDone: {IsDone}, " +
        $"TaskId: {TaskId}, UserId: {UserId}";
}
