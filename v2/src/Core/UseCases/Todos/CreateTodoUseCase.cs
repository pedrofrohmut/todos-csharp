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

    public async Task<Result<CreateTodoOutput>> Execute(CreateTodoInput input)
    {
        Result<CreateTodoOutput> result;

        // Validate input
        result = (Result<CreateTodoOutput>) TodoEntity.ValidateName(input.Name);
        if (!result.IsSuccess) return result;
        result = (Result<CreateTodoOutput>) TodoEntity.ValidateDescription(input.Description);
        if (!result.IsSuccess) return result;

        // Check token and find user by token user id
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Result<CreateTodoOutput>.Failed(resultToken.Error!);
        UserDb user = resultToken.Payload;

        // Create Todo
        var command = new CreateTodoCommand {
            Name = input.Name,
            Description = input.Description,
            UserId = user.Id,
        };
        result = (Result<CreateTodoOutput>) await TodoEntity.CreateTodo(command, this.todoCommandHandler);
        if (!result.IsSuccess) return result;

        return Result<CreateTodoOutput>.Successed(new CreateTodoOutput {});
    }
}
