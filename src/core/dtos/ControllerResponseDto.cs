namespace Todos.Core.Dtos;

public class ControllerResponseDto<T>
{
    public T? Body { get; init; }
    public string Msg { get; init; } = "";
    public int Status { get; init; }
}
