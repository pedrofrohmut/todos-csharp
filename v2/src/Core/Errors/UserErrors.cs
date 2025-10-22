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

public class UserNotFoundError : ResultError
{
    public UserNotFoundError() : base("User:NotFound", "User not found") {}
    public UserNotFoundError(string message) : base("User:NotFound", message) {}
}

public class PasswordMatchError : ResultError
{
    const string code = "User:PasswordMatch";
    public PasswordMatchError() : base(code, "User password and password hash do not match") {}
    public PasswordMatchError(string message) : base(code, message) {}
}
