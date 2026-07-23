namespace Todos.Core.Queries;

public readonly struct ItemFindAllByTodoIdQuery
{
    public int TodoId { get; init; }
}
