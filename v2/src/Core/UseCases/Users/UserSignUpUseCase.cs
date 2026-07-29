using Todos.Core.Utils;
using Todos.Core.Entities;
using Todos.Core.Services;
using Todos.Core.Commands;
using Todos.Core.Queries.Handlers;
using Todos.Core.Commands.Handlers;
using Todos.Core.Queries;
using Todos.Core.Errors;

namespace Todos.Core.UseCases.Users;

public readonly struct UserSignUpInput
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}

public readonly struct UserSignUpOutput {}

public enum UserSignUpErrors
{
    InvalidUser,
    EmailAlreadyTaken,
    Unexpected,
}

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

    private Result<UserSignUpOutput> ErrorCast(Enum errorEnum, Result result)
    {
        var resultError = new ResultError(errorEnum, result.Error.Message);
        return Result<UserSignUpOutput>.Fail(resultError);
    }

    private Result<UserSignUpOutput> ErrorCast<T>(Enum errorEnum, Result<T> result)
    {
        var resultError = new ResultError(errorEnum, result.Error.Message);
        return Result<UserSignUpOutput>.Fail(resultError);
    }

    public async Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = UserEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return ErrorCast(UserSignUpErrors.InvalidUser, validationResult);
        }
        validationResult = UserEntity.ValidateEmail(input.Email);
        if (!validationResult.IsSuccess) {
            return ErrorCast(UserSignUpErrors.InvalidUser, validationResult);
        }
        validationResult = UserEntity.ValidatePassword(input.Password);
        if (!validationResult.IsSuccess) {
            return ErrorCast(UserSignUpErrors.InvalidUser, validationResult);
        }

        // Checks if e-mail is available
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result checkResult = await UserEntity.CheckEmailIsAvailable(query, this.userQueryHandler);
        if (!checkResult.IsSuccess) {
            return ErrorCast(UserSignUpErrors.EmailAlreadyTaken, checkResult);
        }

        // Generate password hash
        Result<string> resultHash = UserEntity.HashPassword(input.Password, this.passwordService);
        if (!resultHash.IsSuccess) {
            return ErrorCast(UserSignUpErrors.Unexpected, resultHash);
        }

        // Create User
        var command = new CreateUserCommand {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = resultHash.Payload,
        };
        Result createResult = await UserEntity.CreateUser(command, this.userCommandHandler);
        if (!createResult.IsSuccess) {
            return ErrorCast(UserSignUpErrors.Unexpected, createResult);
        }

        return Result<UserSignUpOutput>.Ok(new UserSignUpOutput {});
    }
}
