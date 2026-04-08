namespace Todos.Core.Errors;

public class InvalidTodoError : ResultError
{
    const string code = "Todo:InvalidTodo";
    public InvalidTodoError() : base(code, "Todo is invalid") {}
    public InvalidTodoError(string message) : base(code, message) {}
}

public class TodoNotFoundError : ResultError
{
    const string code = "Todo:TodoNotFound";
    public TodoNotFoundError() : base(code, "Todo not found") {}
    public TodoNotFoundError(string message) : base(code, message) {}
}

public class TodoOwnershipError : ResultError
{
    const string code = "Todo:TodoOwnershipError";
    public TodoOwnershipError() : base(code, "This Todo does not belong to this user") {}
    public TodoOwnershipError(string message) : base(code, message) {}
}
