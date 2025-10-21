namespace Todos.Core.Db;

public readonly struct UserDb
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; init; }
}
