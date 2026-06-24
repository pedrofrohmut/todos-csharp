using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Todos;

public readonly struct DeleteTodoInput
{
    public int Id { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct DeleteTodoOutput
{
}

public class DeleteTodoUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;
    private readonly ITodoCommandHandler todoCommandHandler;

    public DeleteTodoUseCase(IAuthTokenService authTokenService,
                             IUserQueryHandler userQueryHandler,
                             ITodoQueryHandler todoQueryHandler,
                             ITodoCommandHandler todoCommandHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
        this.todoCommandHandler = todoCommandHandler;
    }

    private Result<DeleteTodoOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<DeleteTodoOutput>.Fail(result.Error);
    }

    private Result<DeleteTodoOutput> ErrorCast(Result result)
    {
        return Result<DeleteTodoOutput>.Fail(result.Error);
    }

    public async Task<Result<DeleteTodoOutput>> Execute(DeleteTodoInput input)
    {
        // Validate Input
        Result validationResult = TodoEntity.ValidateId(input.Id);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Get user info from token and find userDb with it
        Result<UserDb> getUserResult =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!getUserResult.IsSuccess) {
            return ErrorCast(getUserResult);
        }
        UserDb user = getUserResult.Payload;

        // Checks if todo exists
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> findResult = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(getUserResult);
        }
        TodoDb todo = findResult.Payload;

        // Check todo ownership
        Result ownershipResult = TodoEntity.CheckTodoOwnership(user, todo);
        if (!ownershipResult.IsSuccess) {
            return ErrorCast(ownershipResult);
        }

        // Delete todo
        var command = new TodoDeleteCommand {
            Id = input.Id,
        };
        Result deleteResult = await TodoEntity.DeleteTodo(command, this.todoCommandHandler);
        if (!deleteResult.IsSuccess) {
            return ErrorCast(deleteResult);
        }

        return Result<DeleteTodoOutput>.Ok(new DeleteTodoOutput {});
    }
}
