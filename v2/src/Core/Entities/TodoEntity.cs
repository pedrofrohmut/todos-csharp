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
    public static Result<bool> ValidateId(int id)
    {
        if (id < 1) {
            return Result<bool>.Fail(new InvalidTodoError("Invalid todo id. Id cannot be less than 1."));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result<bool>.Fail(new InvalidTodoError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result<bool>.Fail(new InvalidTodoError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result<bool>.Fail(new InvalidTodoError("Name is too long. Name must be less than 121 characters long."));
        }
        if (!EntitiesUtils.IsValidName(name)) {
            return Result<bool>.Fail(new InvalidTodoError("Name contains invalid characters."));
        }
        return Result<bool>.Ok();
    }

    public static Result<bool> ValidateDescription(string description)
    {
        if (description.Length < 3) {
            return Result<bool>.Fail(new InvalidTodoError("Description is too short. Description must be at least 3 characters long."));
        }
        if (description.Length > 255) {
            return Result<bool>.Fail(new InvalidTodoError("Description is too long. Description must be less than 256 characters long."));
        }
        return Result<bool>.Ok();
    }

    public static async Task<Result<bool>> CreateTodo(TodoCreateCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.CreateTodo(command);
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("Todo:" + nameof(CreateTodo), "Error to create todo: " + e.Message);
        }
    }

    public static async Task<Result<bool>> UpdateTodo(TodoUpdateCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.UpdateTodo(command);
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("Todo:" + nameof(UpdateTodo), "Error to update todo: " + e.Message);
        }
    }

    public static async Task<Result<TodoDb>> FindTodoById(TodoFindByIdQuery query, ITodoQueryHandler handler)
    {
        try {
            TodoDb? todo = await handler.FindTodoById(query);
            if (todo == null) {
                return Result<TodoDb>.Fail(new TodoNotFoundError("Todo not found by id"));
            }
            return Result<TodoDb>.Ok(todo.Value);
        } catch (Exception e) {
            return Result<TodoDb>.Fail("Todo:" + nameof(FindTodoById), "Error to find todo by id: " + e.Message);
        }
    }

    public static async Task<Result<IEnumerable<TodoDb>>> FindAllTodos(TodoFindAllQuery query, ITodoQueryHandler handler)
    {
        try {
            IEnumerable<TodoDb> todos = await handler.FindAllTodos(query);
            return Result<IEnumerable<TodoDb>>.Ok(todos);
        } catch (Exception e) {
            return Result<IEnumerable<TodoDb>>.Fail("Todo:" + nameof(FindAllTodos), "Error to find all todos: " + e.Message);
        }
    }

    public static async Task<Result<bool>> DeleteTodo(TodoDeleteCommand command, ITodoCommandHandler handler)
    {
        try {
            await handler.DeleteTodo(command);
            return Result<bool>.Ok();
        } catch (Exception e) {
            return Result<bool>.Fail("Todo:" + nameof(DeleteTodo), "Error to delete todo: " + e.Message);
        }
    }

    public static Result<bool> CheckTodoOwnership(UserDb user, TodoDb todo)
    {
        if (user.Id == todo.UserId) {
            return Result<bool>.Ok();
        }
        return Result<bool>.Fail(new TodoOwnershipError());
    }

}
