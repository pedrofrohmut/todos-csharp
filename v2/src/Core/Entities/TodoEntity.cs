using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Errors;
using Todos.Core.Utils;

namespace Todos.Core.Entities;

public class TodoEntity
{
    public static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result.Failed(new InvalidTodoError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result.Failed(new InvalidTodoError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result.Failed(new InvalidTodoError("Name is too long. Name must be less than 121 characters long."));
        }
        return Result.Successed();
    }

    public static Result ValidateDescription(string description)
    {
        if (description.Length < 3) {
            return Result.Failed(new InvalidTodoError("Description is too short. Description must be at least 3 characters long."));
        }
        if (description.Length > 255) {
            return Result.Failed(new InvalidTodoError("Description is too long. Description must be less than 256 characters long."));
        }
        return Result.Successed();
    }

    public static async Task<Result> CreateTodo(CreateTodoCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.CreateTodo(command);
            return Result.Successed();
        } catch (Exception e) {
            return Result.Failed("Todo:" + nameof(CreateTodo), "Error to create todo: " + e.Message);
        }
    }
}
