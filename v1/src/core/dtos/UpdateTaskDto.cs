namespace Todos.Core.Dtos;

public class UpdateTaskDto
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";

    public override string ToString() => $"Name: {Name}, Description: {Description}";
}
