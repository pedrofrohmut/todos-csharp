namespace Todos.Core.Errors;

// TODO: Change the codes to an Enum when possible

public class InvalidUserError : ResultError
{
    const string code = "User:InvalidUser";
    public InvalidUserError() : base(code, "User is invalid") {}
    public InvalidUserError(string message) : base(code, message) {}
}

public class EmailAlreadyTakenError : ResultError
{
    const string code = "User:EmailAlreadyTakenError";
    public EmailAlreadyTakenError() : base(code, "User e-mail is already taken and must be unique") {}
    public EmailAlreadyTakenError(string message) : base(code, message) {}
}

public class UserNotFoundError : ResultError
{
    const string code = "User:NotFound";
    public UserNotFoundError() : base(code, "User not found") {}
    public UserNotFoundError(string message) : base(code, message) {}
}

public class PasswordMatchError : ResultError
{
    const string code = "User:PasswordMatch";
    public PasswordMatchError() : base(code, "User password and password hash do not match") {}
    public PasswordMatchError(string message) : base(code, message) {}
}

public class InvalidTokenError : ResultError
{
    const string code = "User:InvalidToken";
    public InvalidTokenError() : base(code, "Authentication token is not valid") {}
    public InvalidTokenError(string message) : base(code, message) {}
}
