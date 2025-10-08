namespace Todos.Core.Dtos;

public class CreateTodoDto
{
    public string Name        { get; init; } = "";
    public string Description { get; init; } = "";
    public bool   IsDone      { get; init; } = false;
    public string TaskId      { get; init; } = "";
    public string UserId      { get; init; } = "";

    public override string ToString() => 
        $"Name: {Name}, Description: {Description}, IsDone: {IsDone}," +
        $" TaskId: {TaskId}, UserId: {UserId}";
}
