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

    private Result<FindAllTodosOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<FindAllTodosOutput>.Fail(result.Error);
    }

    public async Task<Result<FindAllTodosOutput>> Execute(FindAllTodosInput input)
    {
        // Get user info from token and get user from db
        Result<UserDb> getUserResult =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!getUserResult.IsSuccess) {
            return ErrorCast(getUserResult);
        }
        UserDb user = getUserResult.Payload;

        // Find all todos
        var query = new TodoFindAllQuery {
            UserId = input.UserId,
        };
        Result<IEnumerable<TodoDb>> findResult = await TodoEntity.FindAllTodos(query, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        IEnumerable<TodoDb> todos = findResult.Payload!;

        // Return the valid output
        var output = new FindAllTodosOutput {
            Todos = todos.Select(x => new TodoOutput(x)).ToList(),
        };
        return Result<FindAllTodosOutput>.Ok(output);
    }
}
