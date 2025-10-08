namespace Todos.Core.Dtos;

public class UpdateTodoDto
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public bool   IsDone { get; init; } = false;

    public override string ToString() => 
        $"Name: {Name}, Description: {Description}, IsDone: {IsDone}";
}
