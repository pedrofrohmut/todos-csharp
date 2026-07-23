namespace Todos.Core.Db;

public readonly struct ItemDb
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsDone { get; init; }

    public int TodoId { get; init; }
    public int UserId { get; init; }
}
