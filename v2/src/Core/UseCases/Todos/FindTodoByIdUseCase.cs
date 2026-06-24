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

        // Get user info from token and find userDb with it
        Result<UserDb> getUserResult =
            await UserEntity.GetUserFromToken(input.AuthToken, this.authTokenService, this.userQueryHandler);
        if (!getUserResult.IsSuccess) {
            return ErrorCast(getUserResult);
        }
        UserDb user = getUserResult.Payload;

        // Find to by id
        var query = new TodoFindByIdQuery {
            Id = input.Id,
        };
        Result<TodoDb> findResult = await TodoEntity.FindTodoById(query, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        TodoDb todo = findResult.Payload;

        // Check todo ownership
        Result ownershipResult = TodoEntity.CheckTodoOwnership(user, todo);
        if (!ownershipResult.IsSuccess) {
            return ErrorCast(ownershipResult);
        }

        var output = new FindTodoByIdOutput {
            Todo = new TodoOutput(todo),
        };
        return Result<FindTodoByIdOutput>.Ok(output);
    }
}
