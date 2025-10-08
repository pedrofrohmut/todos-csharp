namespace Todos.Core.Exceptions;

public class TodoNotFoundException : Exception
{
    public TodoNotFoundException() : base("Todo not found") {}

    public TodoNotFoundException(string message) : base(message) {}
}
