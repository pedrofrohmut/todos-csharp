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

public readonly struct DeleteTodoOutput {}

public enum DeleteTodoErrors
{
    InvalidTodo,
    InvalidAuthToken,
    UserNotFound,
    TodoNotFound,
    Ownership,
    Unexpected,
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

    public async Task<Result<DeleteTodoOutput>> Execute(DeleteTodoInput input)
    {
        // Validate Input
        Result validationResult = TodoEntity.ValidateId(input.Id);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.InvalidTodo);
        }

        // Get and validate auth token
        Result<AuthToken> getTokenResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!getTokenResult.IsSuccess) {
            return getTokenResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.InvalidAuthToken);
        }
        AuthToken authToken = getTokenResult.Payload;
        validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.InvalidAuthToken);
        }
        int userId = authToken.UserId;

        // Get user from token
        var findUserQuery = new UserFindByIdQuery { Id = userId };
        Result<UserDb> findUserResult = await UserEntity.FindUserById(findUserQuery, this.userQueryHandler);
        if (!findUserResult.IsSuccess) {
            return findUserResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.UserNotFound);
        }
        UserDb userDb = findUserResult.Payload;

        // Checks if todo exists
        var findTodoQuery = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> findTodoResult = await TodoEntity.FindTodoById(findTodoQuery, this.todoQueryHandler);
        if (!findTodoResult.IsSuccess) {
            return findTodoResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.TodoNotFound);
        }
        TodoDb todo = findTodoResult.Payload;

        // Check todo ownership
        Result ownershipResult = TodoEntity.CheckTodoOwnership(userDb, todo);
        if (!ownershipResult.IsSuccess) {
            return ownershipResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.Ownership);
        }

        // Delete todo
        var command = new TodoDeleteCommand {
            Id = input.Id,
        };
        Result deleteResult = await TodoEntity.DeleteTodo(command, this.todoCommandHandler);
        if (!deleteResult.IsSuccess) {
            return deleteResult.ErrorCast<DeleteTodoOutput>(DeleteTodoErrors.Unexpected);
        }

        return Result<DeleteTodoOutput>.Ok(new DeleteTodoOutput {});
    }
}
