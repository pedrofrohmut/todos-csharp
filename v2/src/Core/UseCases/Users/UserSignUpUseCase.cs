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

    public async Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        // Validate input
        Result validationResult;
        validationResult = UserEntity.ValidateName(input.Name);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<UserSignUpOutput>(UserSignUpErrors.InvalidUser);
        }
        validationResult = UserEntity.ValidateEmail(input.Email);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<UserSignUpOutput>(UserSignUpErrors.InvalidUser);
        }
        validationResult = UserEntity.ValidatePassword(input.Password);
        if (!validationResult.IsSuccess) {
            return validationResult.ErrorCast<UserSignUpOutput>(UserSignUpErrors.InvalidUser);
        }

        // Checks if e-mail is available
        var query = new UserFindByEmailQuery { Email = input.Email };
        Result checkEmailResult = await UserEntity.CheckEmailIsAvailable(query, this.userQueryHandler);
        if (!checkEmailResult.IsSuccess) {
            return checkEmailResult.ErrorCast<UserSignUpOutput>(UserSignUpErrors.EmailAlreadyTaken);
        }

        // Generate password hash
        Result<string> resultHashPassword = UserEntity.HashPassword(input.Password, this.passwordService);
        if (!resultHashPassword.IsSuccess) {
            return resultHashPassword.ErrorCast<UserSignUpOutput>(UserSignUpErrors.Unexpected);
        }

        // Create User
        var command = new CreateUserCommand {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = resultHash.Payload,
        };
        Result createUserResult = await UserEntity.CreateUser(command, this.userCommandHandler);
        if (!createUserResult.IsSuccess) {
            return createUserResult.ErrorCast<UserSignUpOutput>(UserSignUpErrors.Unexpected);
        }

        return Result<UserSignUpOutput>.Ok(new UserSignUpOutput {});
    }
}
