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

    private Result<FindTodoByIdOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<FindTodoByIdOutput>.Fail(result.Error);
    }

    private Result<FindTodoByIdOutput> ErrorCast(Result result)
    {
        return Result<FindTodoByIdOutput>.Fail(result.Error);
    }

    public async Task<Result<FindTodoByIdOutput>> Execute(FindTodoByIdInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = TodoEntity.ValidateId(input.Id);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Get auth token from jwt string
        Result<AuthToken> tokenResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!tokenResult.IsSuccess) {
            return ErrorCast(tokenResult);
        }
        AuthToken authToken = tokenResult.Payload;

        // Validate token
        validationResult = UserEntity.ValidateAuthToken(authToken);
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

        // Find to by id
        var findTodoQuery = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> findResult = await TodoEntity.FindTodoById(findTodoQuery, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        TodoDb todo = findResult.Payload;

        // Check todo ownership
        Result ownershipResult = TodoEntity.CheckTodoOwnership(userDb, todo);
        if (!ownershipResult.IsSuccess) {
            return ErrorCast(ownershipResult);
        }

        var output = new FindTodoByIdOutput {
            Todo = new TodoOutput(todo),
        };
        return Result<FindTodoByIdOutput>.Ok(output);
    }
}
