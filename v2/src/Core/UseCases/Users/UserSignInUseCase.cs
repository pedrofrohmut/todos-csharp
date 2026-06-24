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
    public string AuthToken { get; init; }
}

public class UserSignInUseCase
{
    private readonly IUserQueryHandler userQueryHandler;
    private readonly IPasswordService passwordService;
    private readonly IAuthTokenService authTokenService;

    public UserSignInUseCase(
        IUserQueryHandler userQueryHandler,
        IPasswordService passwordService,
        IAuthTokenService authTokenService)
    {
        this.userQueryHandler = userQueryHandler;
        this.passwordService = passwordService;
        this.authTokenService = authTokenService;
    }

    private Result<UserSignInOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<UserSignInOutput>.Fail(result.Error);
    }

    private Result<UserSignInOutput> ErrorCast(Result result)
    {
        return Result<UserSignInOutput>.Fail(result.Error);
    }

    public async Task<Result<UserSignInOutput>> Execute(UserSignInInput input)
    {
        //Validate Input
        Result validationResult;
        validationResult = UserEntity.ValidateEmail(input.Email);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = UserEntity.ValidatePassword(input.Password);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Find user by e-mail
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result<UserDb> findResult = await UserEntity.FindUserByEmail(query, this.userQueryHandler);
        if (!findResult.IsSuccess) {
            return ErrorCast(findResult);
        }
        UserDb userDb = findResult.Payload;

        // Check if input password and userDb passwordHash match
        Result matchResult =
            UserEntity.MatchPasswordAndHash(input.Password, userDb.PasswordHash, this.passwordService);
        if (!matchResult.IsSuccess) {
            return ErrorCast(matchResult);
        }

        // Generates a JWT with the userId
        Result<string> tokenResult = UserEntity.GenerateAuthToken(userDb.Id, this.authTokenService);
        if (!tokenResult.IsSuccess) {
            return ErrorCast(tokenResult);
        }
        string token = tokenResult.Payload!;

        var output = new UserSignInOutput {
            Id = userDb.Id,
            Name = userDb.Name,
            Email = userDb.Email,
            AuthToken = token,
        };
        return Result<UserSignInOutput>.Ok(output);
    }
}
