namespace Todos.Core.Dtos;

public class DecodedTokenDto
{
    public string UserId { get; init; } = "";

    public override string ToString() => $"UserId: {UserId}";
}
