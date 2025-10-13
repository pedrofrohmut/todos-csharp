namespace Todos.Core.Errors;

public class InvalidUserError : ResultError
{
    public InvalidUserError(string message) : base("User:InvalidUser", message) {}
}

public class EmailAlreadyTakenError : ResultError
{
    public EmailAlreadyTakenError() :
        base("User:EmailAlreadyTakenError", "User e-mail is already taken and must be unique") {}
}
