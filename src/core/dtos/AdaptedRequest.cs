namespace Todos.Core.Dtos;

public class AdaptedRequest<T>
{
    public T? Body { get; init; }
    public string? AuthUserId { get; init; }
    public string? param { get; init; }
}
