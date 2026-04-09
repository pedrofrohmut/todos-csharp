using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Todos;

public readonly struct FindTodoByIdInput
{
    public int Id { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct FindTodoByIdOutput
{
    public TodoOutput Todo { get; init; }
}

public class FindTodoByIdUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;

    public FindTodoByIdUseCase(IAuthTokenService authTokenService,
                             IUserQueryHandler userQueryHandler,
                             ITodoQueryHandler todoQueryHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
    }

    public async Task<Result<FindTodoByIdOutput>> Execute(FindTodoByIdInput input)
    {
        Result<FindTodoByIdOutput> result;

        // Validate input
        result = (Result<FindTodoByIdOutput>) TodoEntity.ValidateId(input.Id);
        if (!result.IsSuccess) return result;

        // Check auth token
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Result<FindTodoByIdOutput>.Failed(resultToken.Error!);
        UserDb user = resultToken.Payload;

        // Find to by id
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> resultTodo = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!resultTodo.IsSuccess) return Result<FindTodoByIdOutput>.Failed(resultTodo.Error!);
        TodoDb todo = resultTodo.Payload;

        // Check todo ownership
        result = (Result<FindTodoByIdOutput>) TodoEntity.CheckTodoOwnership(user, todo);
        if (!result.IsSuccess) return result;

        var output = new FindTodoByIdOutput {
            Todo = new TodoOutput(todo),
        };
        return Result<FindTodoByIdOutput>.Succeeded(output);
    }
}
