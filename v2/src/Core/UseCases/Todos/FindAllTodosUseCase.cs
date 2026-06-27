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

    private Result<FindAllTodosOutput> ErrorCast(Result result)
    {
        return Result<FindAllTodosOutput>.Fail(result.Error);
    }

    private Result<FindAllTodosOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<FindAllTodosOutput>.Fail(result.Error);
    }

    public async Task<Result<FindAllTodosOutput>> Execute(FindAllTodosInput input)
    {
        // Get auth token from jwt string
        Result<AuthToken> tokenResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!tokenResult.IsSuccess) {
            return ErrorCast(tokenResult);
        }
        AuthToken authToken = tokenResult.Payload;

        // Validate token
        Result validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Get db user with the jwt userId
        var findUserQuery = new UserFindByIdQuery {
            Id = authToken.UserId,
        };
        Result<UserDb> dbResult = await UserEntity.FindUserById(findUserQuery, this.userQueryHandler);
        if (!dbResult.IsSuccess) {
            return ErrorCast(dbResult);
        }
        UserDb userDb = dbResult.Payload;

        // Find all todos
        var findTodosQuery = new TodoFindAllQuery {
            UserId = userDb.Id,
        };
        Result<IEnumerable<TodoDb>> findResult = await TodoEntity.FindAllTodos(findTodosQuery, this.todoQueryHandler);
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
