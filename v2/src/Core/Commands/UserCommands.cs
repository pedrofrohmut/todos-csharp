namespace Todos.Core.Commands;

public readonly struct CreateUserCommand
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; init; }
}
