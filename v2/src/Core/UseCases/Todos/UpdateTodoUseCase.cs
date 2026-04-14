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

    public async Task<Result<UpdateTodoOutput>> Execute(UpdateTodoInput input)
    {
        Result<UpdateTodoOutput> result;

        // Validate input
        result = (Result<UpdateTodoOutput>) TodoEntity.ValidateId(input.Id);
        if (!result.IsSuccess) return result;
        result = (Result<UpdateTodoOutput>) TodoEntity.ValidateName(input.Name);
        if (!result.IsSuccess) return result;
        result = (Result<UpdateTodoOutput>) TodoEntity.ValidateDescription(input.Description);
        if (!result.IsSuccess) return result;

        // Check auth token
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Result<UpdateTodoOutput>.Failed(resultToken.Error!);
        UserDb user = resultToken.Payload;

        // Find todo by id
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> resultTodo = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!resultTodo.IsSuccess) return Result<UpdateTodoOutput>.Failed(resultTodo.Error!);
        TodoDb todo = resultTodo.Payload;

        // Check todo ownership
        result = (Result<UpdateTodoOutput>) TodoEntity.CheckTodoOwnership(user, todo);
        if (!result.IsSuccess) return result;

        // Update todo
        var command = new TodoUpdateCommand {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
        };
        result = (Result<UpdateTodoOutput>) await TodoEntity.UpdateTodo(command, this.todoCommandHandler);
        if (!result.IsSuccess) return result;

        return Result<UpdateTodoOutput>.Succeeded(new UpdateTodoOutput {});
    }
}
