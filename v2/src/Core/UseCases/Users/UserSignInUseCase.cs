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

// TODO: Generate and return authToken
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

    private Result<UserSignInOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<UserSignInOutput>.Fail(result.Error);
    }

    public async Task<Result<UserSignInOutput>> Execute(UserSignInInput input)
    {
        //Validate Input
        Result<bool> validationResult;
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
        Result<bool> matchResult =
            UserEntity.MatchPasswordAndHash(input.Password, userDb.PasswordHash, this.passwordService);
        if (!matchResult.IsSuccess) {
            return ErrorCast(matchResult);
        }

        // TODO: generate a JWT to send with the output

        var output = new UserSignInOutput { Id = userDb.Id, Name = userDb.Name, Email = userDb.Email };
        return Result<UserSignInOutput>.Ok(output);
    }
}
