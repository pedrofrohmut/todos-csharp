namespace Todos.Core.Commands;

public readonly struct CreateTodoCommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int UserId { get; init; }
}
