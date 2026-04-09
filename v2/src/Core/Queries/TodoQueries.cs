namespace Todos.Core.Queries;

public readonly struct TodoFindByIdQuery
{
    public int Id { get; init; }
}

public readonly struct TodoFindAllQuery
{
    public int UserId { get; init; }
}
