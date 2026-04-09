using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Db;
using Todos.Core.Errors;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Utils;

namespace Todos.Core.Entities;

public class TodoEntity
{
    public static Result ValidateId(int id)
    {
        if (id < 1) {
            return Result.Failed(new InvalidTodoError("Invalid todo id. Id cannot be less than 1."));
        }
        return Result.Succeeded();
    }

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
        if (!EntitiesUtils.IsValidName(name)) {
            return Result.Failed(new InvalidTodoError("Name contains invalid characters."));
        }
        return Result.Succeeded();
    }

    public static Result ValidateDescription(string description)
    {
        if (description.Length < 3) {
            return Result.Failed(new InvalidTodoError("Description is too short. Description must be at least 3 characters long."));
        }
        if (description.Length > 255) {
            return Result.Failed(new InvalidTodoError("Description is too long. Description must be less than 256 characters long."));
        }
        return Result.Succeeded();
    }

    public static async Task<Result> CreateTodo(TodoCreateCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.CreateTodo(command);
            return Result.Succeeded();
        } catch (Exception e) {
            return Result.Failed("Todo:" + nameof(CreateTodo), "Error to create todo: " + e.Message);
        }
    }

    public static async Task<Result<TodoDb>> FindTodoById(TodoFindByIdQuery query, ITodoQueryHandler handler)
    {
        try {
            TodoDb? todo = await handler.FindTodoById(query);
            if (todo == null) {
                return Result<TodoDb>.Failed(new TodoNotFoundError("Todo not found by id"));
            }
            return Result<TodoDb>.Succeeded();
        } catch (Exception e) {
            return Result<TodoDb>.Failed("Todo:" + nameof(FindTodoById), "Error to find todo by id: " + e.Message);
        }
    }

    public static async Task<Result> DeleteTodo(TodoDeleteCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.DeleteTodo(command);
            return Result.Succeeded();
        } catch (Exception e) {
            return Result.Failed("Todo:" + nameof(DeleteTodo), "Error to delete todo: " + e.Message);
        }
    }

    public static Result CheckTodoOwnership(UserDb user, TodoDb todo)
    {
        if (user.Id == todo.UserId) {
            return Result.Succeeded();
        }
        return Result.Failed(new TodoOwnershipError());
    }

}
