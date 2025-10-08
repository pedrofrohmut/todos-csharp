namespace Todos.Core.Exceptions;

public class NotResourceOwnerException : Exception
{
    public NotResourceOwnerException() : base("AuthUser is not the resource owner") {}

    public NotResourceOwnerException(string message) : base(message) {}
}
