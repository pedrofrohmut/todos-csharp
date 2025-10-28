namespace Todos.Core.Errors;

public class InvalidTodoError : ResultError
{
    const string code = "Todo:InvalidTodo";
    public InvalidTodoError() : base(code, "Todo is invalid") {}
    public InvalidTodoError(string message) : base(code, message) {}
}
