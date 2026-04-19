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

    private Result<CreateTodoOutput> Cast(Result result) => Result<CreateTodoOutput>.Cast(result);

    public async Task<Result<CreateTodoOutput>> Execute(CreateTodoInput input)
    {
        Result result;

        // Validate input
        result = TodoEntity.ValidateName(input.Name);
        if (!result.IsSuccess) return Cast(result);
        result = TodoEntity.ValidateDescription(input.Description);
        if (!result.IsSuccess) return Cast(result);

        // Check token and find user by token user id
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Cast(resultToken);
        UserDb userDb = resultToken.Payload;

        // Create Todo
        var command = new TodoCreateCommand {
            Name = input.Name,
            Description = input.Description,
            UserId = userDb.Id,
        };
        result = await TodoEntity.CreateTodo(command, this.todoCommandHandler);
        if (!result.IsSuccess) return Cast(result);

        return Result<CreateTodoOutput>.Succeeded(new CreateTodoOutput {});
    }
}
