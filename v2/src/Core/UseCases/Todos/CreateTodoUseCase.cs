using Todos.Core.Utils;
using Todos.Core.Entities;
using Todos.Core.Services;
using Todos.Core.Db;
using Todos.Core.Queries.Handlers;
using Todos.Core.Commands.Handlers;
using Todos.Core.Commands;

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

    public async Task<Result<CreateTodoOutput>> Execute(CreateTodoInput input)
    {
        // Validate input
        Result<bool> validationResult;
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
        UserDb userDb = getUserResult.Payload;

        // Create Todo
        var command = new TodoCreateCommand {
            Name = input.Name,
            Description = input.Description,
            UserId = userDb.Id,
        };
        Result<bool> createResult = await TodoEntity.CreateTodo(command, this.todoCommandHandler);
        if (!createResult.IsSuccess) {
            return ErrorCast(createResult);
        }

        return Result<CreateTodoOutput>.Ok(new CreateTodoOutput {});
    }
}
