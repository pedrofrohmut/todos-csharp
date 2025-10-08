namespace Todos.Core.Exceptions;

public class EmailAlreadyTakenException : Exception
{
    public EmailAlreadyTakenException() : base("User e-mail already taken") {}

    public EmailAlreadyTakenException(string message) : base(message) {}
}
