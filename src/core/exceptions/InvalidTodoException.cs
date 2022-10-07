namespace Todos.Core.Exceptions;

public class InvalidTodoException : Exception
{
    public InvalidTodoException() : base("Todo is invalid") {}
    
    public InvalidTodoException(string message) : base(message) {}
}
