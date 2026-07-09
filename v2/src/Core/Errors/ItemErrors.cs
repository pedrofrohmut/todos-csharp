namespace Todos.Core.Errors;

public class InvalidItemError : ResultError
{
    const string code = "Item:InvalidItem";
    public InvalidItemError() : base(code, "Item is invalid") {}
    public InvalidItemError(string message) : base(code, message) {}
}
