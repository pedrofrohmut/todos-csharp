namespace Todos.Core.Exceptions;

public class PasswordAndHashNotMatchException : Exception
{
    public PasswordAndHashNotMatchException() : base("Password and Hash do not match") {}
    
    public PasswordAndHashNotMatchException(string message) : base(message) {}
}
