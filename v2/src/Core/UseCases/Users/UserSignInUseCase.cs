using Todos.Core.Db;
using Todos.Core.Entities;
using Todos.Core.Errors;
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

public enum UserSignInErrors
{
    InvalidUser,
    PasswordMismatch,
    UserNotFound,
    Unexpected,
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

    public async Task<Result<UserSignInOutput>> Execute(UserSignInInput input)
    {
        //Validate Input
        Result validationResult;
        validationResult = UserEntity.ValidateEmail(input.Email);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<UserSignInOutput>(UserSignInErrors.InvalidUser);
        }
        validationResult = UserEntity.ValidatePassword(input.Password);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<UserSignInOutput>(UserSignInErrors.InvalidUser);
        }

        // Find user by e-mail
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result<UserDb> findUserResult = await UserEntity.FindUserByEmail(query, this.userQueryHandler);
        if (!findUserResult.IsSuccess) {
            return findUserResult.ErrorCast<UserSignInOutput>(UserSignInErrors.UserNotFound);
        }
        UserDb userDb = findUserResult.Payload;

        // Check if input password and userDb passwordHash match
        Result passwordMatchResult =
            UserEntity.MatchPasswordAndHash(input.Password, userDb.PasswordHash, this.passwordService);
        if (!passwordMatchResult.IsSuccess) {
            return passwordMatchResult.ErrorCast<UserSignInOutput>(UserSignInErrors.PasswordMismatch);
        }

        // Generates a JWT with the userId
        Result<string> generateTokenResult = UserEntity.GenerateAuthToken(userDb.Id, this.authTokenService);
        if (!generateTokenResult.IsSuccess) {
            return generateTokenResult.ErrorCast<UserSignInOutput>(UserSignInErrors.Unexpected);
        }
        string token = generateTokenResult.Payload;

        var signInOutput = new UserSignInOutput {
            Id = userDb.Id,
            Name = userDb.Name,
            Email = userDb.Email,
            AuthToken = token,
        };
        return Result<UserSignInOutput>.Ok(signInOutput);
    }
}
