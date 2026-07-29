namespace Todos.Core.Errors;

public enum UserErrors
{
    Invalid,
    EmailAlreadyTaken,
    NotFound,
    PasswordMismatch,
    InvalidToken,
}

public class InvalidUserError : ResultError
{
    private readonly static Enum code = UserErrors.Invalid;
    public InvalidUserError() : base(code, "User is invalid") {}
    public InvalidUserError(string message) : base(code, message) {}
}

public class EmailAlreadyTakenError : ResultError
{
    private readonly static Enum code = UserErrors.EmailAlreadyTaken;
    public EmailAlreadyTakenError() : base(code, "User e-mail is already taken and must be unique") {}
    public EmailAlreadyTakenError(string message) : base(code, message) {}
}

public class UserNotFoundError : ResultError
{
    private readonly static Enum code = UserErrors.NotFound;
    public UserNotFoundError() : base(code, "User not found") {}
    public UserNotFoundError(string message) : base(code, message) {}
}

public class PasswordMismatchError : ResultError
{
    private readonly static Enum code = UserErrors.PasswordMismatch;
    public PasswordMismatchError() : base(code, "User password and password hash do not match") {}
    public PasswordMismatchError(string message) : base(code, message) {}
}

public class InvalidTokenError : ResultError
{
    private readonly static Enum code = UserErrors.InvalidToken;
    public InvalidTokenError() : base(code, "Authentication token is not valid") {}
    public InvalidTokenError(string message) : base(code, message) {}
}
