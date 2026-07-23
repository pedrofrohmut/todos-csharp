using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Items;

public readonly struct FindAllItemsByTodoIdInput
{
    public int TodoId { get; init; }
    public string? AuthToken { get; init; }
}

public readonly struct FindAllItemsByTodoIdOutput
{
    public IEnumerable<ItemOutput> Items { get; init; }
}

public class FindAllItemsByTodoIdUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;
    private readonly IItemQueryHandler itemQueryHandler;

    public FindAllItemsByTodoIdUseCase(
            IAuthTokenService authTokenService,
            IUserQueryHandler userQueryHandler,
            ITodoQueryHandler todoQueryHandler,
            IItemQueryHandler itemQueryHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
        this.itemQueryHandler = itemQueryHandler;
    }

    public Result<FindAllItemsByTodoIdOutput> ErrorCast(Result result)
    {
        return Result<FindAllItemsByTodoIdOutput>.Fail(result.Error);
    }

    public Result<FindAllItemsByTodoIdOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<FindAllItemsByTodoIdOutput>.Fail(result.Error);
    }

    public async Task<Result<FindAllItemsByTodoIdOutput>> Execute(FindAllItemsByTodoIdInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = ItemEntity.ValidateId(input.TodoId);
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
            Id = input.TodoId,
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

        // Find all items by todo id
        var findItemsQuery = new ItemFindAllByTodoIdQuery {
            TodoId = input.TodoId,
        };
        Result<IEnumerable<ItemDb>> findItemsResult =
            await ItemEntity.FindAllItemsByTodoId(findItemsQuery, this.itemQueryHandler);
        if (!findItemsResult.IsSuccess) {
            return ErrorCast(findItemsResult);
        }
        IEnumerable<ItemDb> items = findItemsResult.Payload;

        var output = new FindAllItemsByTodoIdOutput {
            Items = items.Select(x => new ItemOutput(x)).ToList(),
        };
        return Result<FindAllItemsByTodoIdOutput>.Ok(output);
    }
}
