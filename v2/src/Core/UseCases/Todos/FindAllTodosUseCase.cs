using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Todos;

public readonly struct FindAllTodosInput
{
    public int UserId { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct FindAllTodosOutput
{
    public IEnumerable<TodoOutput> Todos { get; init; }
}

public class FindAllTodosUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;

    public FindAllTodosUseCase(IAuthTokenService authTokenService,
                             IUserQueryHandler userQueryHandler,
                             ITodoQueryHandler todoQueryHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
    }

    public async Task<Result<FindAllTodosOutput>> Execute(FindAllTodosInput input)
    {
        // Check auth token
        Result<UserDb> resultToken =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!resultToken.IsSuccess) return Result<FindAllTodosOutput>.Failed(resultToken.Error!);
        UserDb user = resultToken.Payload;

        // Find all todos
        var query = new TodoFindAllQuery {
            UserId = input.UserId,
        };
        Result<IEnumerable<TodoDb>> resultTodos = await TodoEntity.FindAllTodos(query, this.todoQueryHandler);
        if (!resultTodos.IsSuccess) return Result<FindAllTodosOutput>.Failed(resultTodos.Error!);
        IEnumerable<TodoDb> todos = resultTodos.Payload!;

        // Return the valid output
        var output = new FindAllTodosOutput {
            Todos = todos.Select(x => new TodoOutput(x)).ToList(),
        };
        return Result<FindAllTodosOutput>.Succeeded(output);
    }
}
