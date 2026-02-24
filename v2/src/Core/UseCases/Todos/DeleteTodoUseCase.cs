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

    public async Task<Result<DeleteTodoOutput>> Execute(DeleteTodoInput input)
    {
        Result<DeleteTodoOutput> result;

        // Validate Input
        result = (Result<DeleteTodoOutput>) TodoEntity.ValidateId(input.Id);
        if (!result.IsSuccess) return result;

        // Check auth token
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Result<DeleteTodoOutput>.Failed(resultToken.Error!);
        UserDb user = resultToken.Payload;

        // Check todo exists
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> resultTodo = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!resultTodo.IsSuccess) return Result<DeleteTodoOutput>.Failed(resultToken.Error!);
        TodoDb todo = resultTodo.Payload;

        // Check todo ownership
        // TODO: check todo userId match auth userId

        // Delete todo
        var command = new TodoDeleteCommand {
            Id = input.Id,
        };
        result = (Result<DeleteTodoOutput>) await TodoEntity.DeleteTodo(command, this.todoCommandHandler);

        return Result<DeleteTodoOutput>.Succeeded(new DeleteTodoOutput {});
    }
}
