namespace Todos.Core.Commands;

public readonly struct TodoCreateCommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int UserId { get; init; }
}

public readonly struct TodoDeleteCommand
{
    public int Id { get; init; }
}
