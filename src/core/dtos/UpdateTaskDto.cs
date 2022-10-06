namespace Todos.Core.Dtos;

public class UpdateTaskDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";

    public override string ToString() => 
        $"Id: {Id}, Name: {Name}, Description: {Description}";
}
