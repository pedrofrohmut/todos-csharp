using Todos.Core.Utils;
using Todos.Core.Entities;
using Todos.Core.Services;
using Todos.Core.Db;
using Todos.Core.Queries.Handlers;
using Todos.Core.Commands.Handlers;
using Todos.Core.Commands;
using Todos.Core.Queries;

namespace Todos.Core.UseCases.Todos;

public readonly struct CreateTodoInput
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct CreateTodoOutput {}

public enum CreateTodoErrors
{
    InvalidTodo,
    InvalidAuthToken,
    UserNotFound,
    Unexpected,
}

public class CreateTodoUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoCommandHandler todoCommandHandler;

    public CreateTodoUseCase(IAuthTokenService authTokenService,
                             IUserQueryHandler userQueryHandler,
                             ITodoCommandHandler todoCommandHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoCommandHandler = todoCommandHandler;
    }

    public async Task<Result<CreateTodoOutput>> Execute(CreateTodoInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = TodoEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.InvalidTodo);
        }
        validationResult = TodoEntity.ValidateDescription(input.Description);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.InvalidTodo);
        }

        // Get and validate auth token
        Result<AuthToken> getTokenResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!getTokenResult.IsSuccess) {
            return getTokenResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.InvalidAuthToken);
        }
        AuthToken authToken = getTokenResult.Payload;
        validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.InvalidAuthToken);
        }
        int userId = authToken.UserId;

        // Get user from token
        var query = new UserFindByIdQuery { Id = userId };
        Result<UserDb> findUserResult = await UserEntity.FindUserById(query, this.userQueryHandler);
        if (!findUserResult.IsSuccess) {
            return findUserResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.UserNotFound);
        }
        UserDb userDb = findUserResult.Payload;

        // Create Todo
        var command = new TodoCreateCommand {
            Name = input.Name,
            Description = input.Description,
            UserId = userDb.Id,
        };
        Result createResult = await TodoEntity.CreateTodo(command, this.todoCommandHandler);
        if (!createResult.IsSuccess) {
            return createResult.ErrorCast<CreateTodoOutput>(CreateTodoErrors.Unexpected);
        }

        return Result<CreateTodoOutput>.Ok(new CreateTodoOutput {});
    }
}
