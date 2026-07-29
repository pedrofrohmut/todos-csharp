namespace Todos.Core.Errors;

public enum ItemErrors
{
    Invalid,
}

public class InvalidItemError : ResultError
{
    private readonly static Enum code = ItemErrors.Invalid;
    public InvalidItemError() : base(code, "Item is invalid") {}
    public InvalidItemError(string message) : base(code, message) {}
}
