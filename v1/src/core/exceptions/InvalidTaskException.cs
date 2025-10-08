namespace Todos.Core.Exceptions;

public class InvalidTaskException : Exception
{
    public InvalidTaskException() : base("Task is invalid") {}
    
    public InvalidTaskException(string message) : base(message) {}
}
