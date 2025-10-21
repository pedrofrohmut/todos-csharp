namespace Todos.Core.Queries;

public readonly struct UserFindByEmailQuery
{
    public string Email { get; init; }
}
