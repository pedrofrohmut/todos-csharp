namespace Todos.Core.Dtos;

public class CreateTaskDto
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string UserId { get; init; } = "";

    public override string ToString() => 
        $"Name: {Name}, Description: {Description}, UserId: {UserId}";
}
