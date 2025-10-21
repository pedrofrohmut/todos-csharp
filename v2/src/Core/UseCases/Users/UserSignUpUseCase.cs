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

    public async Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        Result<UserSignUpOutput> result;

        // Validate user
        result = (Result<UserSignUpOutput>) UserEntity.ValidateName(input.Name);
        if (!result.IsSuccess) return result;
        result = (Result<UserSignUpOutput>) UserEntity.ValidateEmail(input.Email);
        if (!result.IsSuccess) return result;
        result = (Result<UserSignUpOutput>) UserEntity.ValidatePassword(input.Password);
        if (!result.IsSuccess) return result;

        // Chech e-mail is available
        var query = new UserFindByEmailQuery { Email = input.Email };
        result = (Result<UserSignUpOutput>) await UserEntity.CheckEmailIsAvailable(query, this.userQueryHandler);
        if (!result.IsSuccess) return result;

        // Hash password
        var resultHash = UserEntity.HashPassword(input.Password, this.passwordService);
        if (!resultHash.IsSuccess || resultHash.Payload == null) {
            return Result<UserSignUpOutput>.Failed(resultHash.Error!);
        }

        // Create User
        var command = new CreateUserCommand {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = resultHash.Payload
        };
        result = (Result<UserSignUpOutput>) await UserEntity.CreateUser(command, this.userCommandHandler);

        return Result<UserSignUpOutput>.Successed();
    }
}
