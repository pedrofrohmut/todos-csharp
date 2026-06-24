using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Todos;

public readonly struct UpdateTodoInput
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct UpdateTodoOutput {}

public class UpdateTodoUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;
    private readonly ITodoCommandHandler todoCommandHandler;

    public UpdateTodoUseCase(
            IAuthTokenService authTokenService,
            IUserQueryHandler userQueryHandler,
            ITodoQueryHandler todoQueryHandler,
            ITodoCommandHandler todoCommandHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
        this.todoCommandHandler = todoCommandHandler;
    }

    private Result<UpdateTodoOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<UpdateTodoOutput>.Fail(result.Error);
    }

    private Result<UpdateTodoOutput> ErrorCast(Result result)
    {
        return Result<UpdateTodoOutput>.Fail(result.Error);
    }

    public async Task<Result<UpdateTodoOutput>> Execute(UpdateTodoInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = TodoEntity.ValidateId(input.Id);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = TodoEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = TodoEntity.ValidateDescription(input.Description);
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

        // Find todo by id
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> findResult = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        TodoDb todo = findResult.Payload;

        // Check todo ownership
        Result ownershiptResult = TodoEntity.CheckTodoOwnership(user, todo);
        if (!ownershiptResult.IsSuccess) {
            return ErrorCast(ownershiptResult);
        }

        // Update todo
        var command = new TodoUpdateCommand {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
        };
        Result updateResult = await TodoEntity.UpdateTodo(command, this.todoCommandHandler);
        if (!updateResult.IsSuccess) {
            return ErrorCast(updateResult);
        }

        return Result<UpdateTodoOutput>.Ok(new UpdateTodoOutput {});
    }
}
