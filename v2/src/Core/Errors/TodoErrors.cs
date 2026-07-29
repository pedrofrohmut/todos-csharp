namespace Todos.Core.Errors;

public enum TodoErrors
{
    Invalid,
    NotFound,
    Ownership,
}

public class InvalidTodoError : ResultError
{
    private readonly static Enum code = TodoErrors.Invalid;
    public InvalidTodoError() : base(code, "Todo is invalid") {}
    public InvalidTodoError(string message) : base(code, message) {}
}

public class TodoNotFoundError : ResultError
{
    private readonly static Enum code = TodoErrors.NotFound;
    public TodoNotFoundError() : base(code, "Todo not found") {}
    public TodoNotFoundError(string message) : base(code, message) {}
}

public class TodoOwnershipError : ResultError
{
    private readonly static Enum code = TodoErrors.Ownership;
    public TodoOwnershipError() : base(code, "This Todo does not belong to this user") {}
    public TodoOwnershipError(string message) : base(code, message) {}
}
