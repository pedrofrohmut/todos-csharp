namespace Todos.Core.Dtos;

public class SignedUserDto
{
    public string UserId { get; init; } = "";
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public string Token { get; init; } = "";
}
