namespace Todos.Core.Exceptions;

public class InvalidRequestAuthException : Exception
{
    public InvalidRequestAuthException() : base("Request Authorization Header is invalid") { }

    public InvalidRequestAuthException(string message) : base(message) { }
}
