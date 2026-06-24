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

public readonly struct CreateTodoOutput
{
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

    private Result<CreateTodoOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<CreateTodoOutput>.Fail(result.Error);
    }

    private Result<CreateTodoOutput> ErrorCast(Result result)
    {
        return Result<CreateTodoOutput>.Fail(result.Error);
    }

    public async Task<Result<CreateTodoOutput>> Execute(CreateTodoInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = TodoEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = TodoEntity.ValidateDescription(input.Description);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Get and validate auth token
        Result<AuthToken> getResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!getResult.IsSuccess) {
            return ErrorCast(getResult);
        }
        AuthToken authToken = getResult.Payload;
        validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        int userId = authToken.UserId;

        // Get user from token
        var query = new UserFindByIdQuery { Id = userId };
        Result<UserDb> findUserResult = await UserEntity.FindUserById(query, this.userQueryHandler);
        if (!findUserResult.IsSuccess) {
            return ErrorCast(findUserResult);
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
            return ErrorCast(createResult);
        }

        return Result<CreateTodoOutput>.Ok(new CreateTodoOutput {});
    }
}
