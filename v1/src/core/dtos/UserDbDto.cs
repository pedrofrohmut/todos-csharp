namespace Todos.Core.Dtos;

public class UserDbDto
{
    public Guid   Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public string PasswordHash { get; init; } = "";

    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, Email: {Email}, PasswordHash: {PasswordHash}";
}
