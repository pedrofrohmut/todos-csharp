namespace Todos.Core.Exceptions;

public class GenerateTokenException : Exception
{
    public GenerateTokenException() : base ("Error to generate token") {}
    
    public GenerateTokenException(string message) : base (message) {}
}
