namespace Todos.Core.Commands;

public readonly struct ItemCreateCommand
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int TodoId { get; init; }
    public int UserId { get; init; }
}

public readonly struct ItemUpdateCommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

public readonly struct ItemDeleteCommand
{
    public int Id { get; init; }
}
