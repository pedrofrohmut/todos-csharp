namespace Todos.Core.Db;

public readonly struct TodoDb
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int UserId { get; init; }
}
