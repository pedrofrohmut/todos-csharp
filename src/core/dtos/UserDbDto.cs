namespace Todos.Core.Dtos;

public class UserDbDto
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public string PasswordHash { get; init; } = "";
}
