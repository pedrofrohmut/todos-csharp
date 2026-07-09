using Todos.Core.Entities;
using Todos.Core.Services;
using Todos.Core.Db;
using Todos.Core.Utils;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Core.UseCases.Items;

public readonly struct CreateItemInput
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int TodoId { get; init; }
    public string AuthToken { get; init; }
}

public readonly struct CreateItemOutput
{
}

public class CreateItemUseCase
{
    private readonly IAuthTokenService authTokenService;
    private readonly IUserQueryHandler userQueryHandler;
    private readonly ITodoQueryHandler todoQueryHandler;
    private readonly IItemCommandHandler itemCommandHandler;

    public CreateItemUseCase(
            IAuthTokenService authTokenService,
            IUserQueryHandler userQueryHandler,
            ITodoQueryHandler todoQueryHandler,
            IItemCommandHandler itemCommandHandler)
    {
        this.authTokenService = authTokenService;
        this.userQueryHandler = userQueryHandler;
        this.todoQueryHandler = todoQueryHandler;
        this.itemCommandHandler = itemCommandHandler;
    }

    private Result<CreateItemOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<CreateItemOutput>.Fail(result.Error);
    }

    private Result<CreateItemOutput> ErrorCast(Result result)
    {
        return Result<CreateItemOutput>.Fail(result.Error);
    }

    public async Task<Result<CreateItemOutput>> Execute(CreateItemInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = ItemEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = ItemEntity.ValidateDescription(input.Description);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = TodoEntity.ValidateId(input.TodoId);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Get and validate auth token
        Result<AuthToken> getResult = UserEntity.GetAuthToken(input.AuthToken, this.authTokenService);
        if (!getResult.IsSuccess) {
            return ErrorCast(getResult);
        }
        AuthToken authToken = getResult.Payload;
        validationResult = UserEntity.ValidateAuthToken(authToken);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        int userId = authToken.UserId;

        // Get user from token
        var query = new UserFindByIdQuery { Id = userId };
        Result<UserDb> findUserResult = await UserEntity.FindUserById(query, this.userQueryHandler);
        if (!findUserResult.IsSuccess) {
            return ErrorCast(findUserResult);
        }
        UserDb userDb = findUserResult.Payload;

        // Get todo by id
        var findQuery = new TodoFindByIdQuery { Id = input.TodoId };
        var findResult = await TodoEntity.FindTodoById(findQuery, this.todoQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        TodoDb todoDb = findResult.Payload;

        // Check todo ownership
        Result ownershipResult = TodoEntity.CheckTodoOwnership(userDb, todoDb);
        if (!ownershipResult.IsSuccess) {
            return ErrorCast(ownershipResult);
        }

        // Create item
        var createCommand = new ItemCreateCommand {
            Name = input.Name,
            Description = input.Description,
            TodoId = input.TodoId,
            UserId = userDb.Id,
        };
        Result createResult = await ItemEntity.CreateItem(createCommand, this.itemCommandHandler);
        if (!createResult.IsSuccess) {
            return ErrorCast(createResult);
        }

        return Result<CreateItemOutput>.Ok(new CreateItemOutput {});
    }
}
