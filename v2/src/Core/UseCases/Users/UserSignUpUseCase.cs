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

    private Result<UserSignUpOutput> Cast(Result result) => Result<UserSignUpOutput>.Cast(result);

    public async Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        Result result;

        // Validate input
        result = UserEntity.ValidateName(input.Name);
        if (!result.IsSuccess) return Cast(result);
        result = UserEntity.ValidateEmail(input.Email);
        if (!result.IsSuccess) return Cast(result);
        result = UserEntity.ValidatePassword(input.Password);
        if (!result.IsSuccess) return Cast(result);
        Console.WriteLine("UserSignUp: input is valid");

        // Chech e-mail is available
        var query = new UserFindByEmailQuery { Email = input.Email };
        result = await UserEntity.CheckEmailIsAvailable(query, this.userQueryHandler);
        if (!result.IsSuccess) return Cast(result);
        Console.WriteLine("UserSignUp: e-mail is available");

        // Hash password
        var resultHash = UserEntity.HashPassword(input.Password, this.passwordService);
        if (!resultHash.IsSuccess) return Cast(resultHash);
        Console.WriteLine("UserSignUp: password is hashed");

        // Create User
        var command = new CreateUserCommand {
            Name = input.Name,
            Email = input.Email,
            PasswordHash = resultHash.Payload!
        };
        result = await UserEntity.CreateUser(command, this.userCommandHandler);
        if (!result.IsSuccess) return Cast(result);
        Console.WriteLine("UserSignUp: user is created");

        return Result<UserSignUpOutput>.Succeeded(new UserSignUpOutput {});
    }
}
