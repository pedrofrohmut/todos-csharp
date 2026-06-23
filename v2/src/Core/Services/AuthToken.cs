namespace Todos.Core.Services;

public readonly struct AuthToken
{
    public int UserId { get; init; }
    public long Expiration { get; init; }
    public long IssuedAt { get; init; }
}
