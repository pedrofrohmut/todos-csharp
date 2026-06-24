using Todos.Core.Utils;
using Todos.Core.Entities;
using Todos.Core.Services;
using Todos.Core.Commands;
using Todos.Core.Queries.Handlers;
using Todos.Core.Commands.Handlers;
using Todos.Core.Queries;

namespace Todos.Core.UseCases.Users;

public readonly struct UserSignUpInput
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}

public readonly struct UserSignUpOutput {}

public class UserSignUpUseCase
{
    private readonly IUserQueryHandler userQueryHandler;
    private readonly IUserCommandHandler userCommandHandler;
    private readonly IPasswordService passwordService;

    public UserSignUpUseCase(IUserQueryHandler userQueryHandler,
                             IUserCommandHandler userCommandHandler,
                             IPasswordService passwordService)
    {
        this.userQueryHandler = userQueryHandler;
        this.userCommandHandler = userCommandHandler;
        this.passwordService = passwordService;
    }

    private Result<UserSignUpOutput> ErrorCast<T>(Result<T> result)
    {
        return Result<UserSignUpOutput>.Fail(result.Error);
    }

    private Result<UserSignUpOutput> ErrorCast(Result result)
    {
        return Result<UserSignUpOutput>.Fail(result.Error);
    }

    public async Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = UserEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = UserEntity.ValidateEmail(input.Email);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }
        validationResult = UserEntity.ValidatePassword(input.Password);
        if (!validationResult.IsSuccess) {
            return ErrorCast(validationResult);
        }

        // Checks if e-mail is available
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result checkResult = await UserEntity.CheckEmailIsAvailable(query, this.userQueryHandler);
        if (!checkResult.IsSuccess) {
            return ErrorCast(checkResult);
        }

        // Generate password hash
        Result<string> resultHash = UserEntity.HashPassword(input.Password, this.passwordService);
        if (!resultHash.IsSuccess) {
            return ErrorCast(resultHash);
        }

        // Create User
        var command = new CreateUserCommand {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = resultHash.Payload!
        };
        Result createResult = await UserEntity.CreateUser(command, this.userCommandHandler);
        if (!createResult.IsSuccess) {
            return ErrorCast(createResult);
        }

        return Result<UserSignUpOutput>.Ok(new UserSignUpOutput {});
    }
}
