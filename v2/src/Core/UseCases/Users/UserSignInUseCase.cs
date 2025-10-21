using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Core.UseCases.Users;

public readonly struct UserSignInInput
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public readonly struct UserSignInOutput
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}

public class UserSignInUseCase
{
    private readonly IUserQueryHandler userQueryHandler;
    private readonly IPasswordService passwordService;

    public UserSignInUseCase(IUserQueryHandler userQueryHandler, IPasswordService passwordService)
    {
        this.userQueryHandler = userQueryHandler;
        this.passwordService = passwordService;
    }

    public async Task<Result<UserSignInOutput>> Execute(UserSignInInput input)
    {
        Result<UserSignInOutput> result;

        // Validate input
        result = (Result<UserSignInOutput>) UserEntity.ValidateEmail(input.Email);
        if (!result.IsSuccess) return result;
        result = (Result<UserSignInOutput>)UserEntity.ValidatePassword(input.Password);
        if (!result.IsSuccess) return result;

        // Get user from persistence
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result<UserDb> resultUser = await UserEntity.FindUserByEmail(query, this.userQueryHandler);
        if (!resultUser.IsSuccess) return Result<UserSignInOutput>.Failed(resultUser.Error!);

        UserDb userDb = resultUser.Payload;

        // Check input password with persistence passwordHash
        result = (Result<UserSignInOutput>) UserEntity.MatchPasswordAndHash(input.Password,
                                                                            userDb.PasswordHash,
                                                                            this.passwordService);
        if (!result.IsSuccess) return result;

        var output = new UserSignInOutput { Id = userDb.Id, Name = userDb.Name, Email = userDb.Email };
        return Result<UserSignInOutput>.Successed(output);
    }
}
