namespace Todos.Core.Queries;

public readonly struct UserFindByEmailQuery
{
    public string Email { get; init; }
}

public readonly struct UserFindByIdQuery
{
    public int Id { get; init; }
}
