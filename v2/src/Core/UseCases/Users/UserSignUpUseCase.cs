using Todos.Core.Utils;
using Todos.Core.DataAccess;
using Todos.Core.Services;

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
    private readonly IUserDataAccess userDataAccess;
    private readonly IPasswordService passwordService;

    public UserSignUpUseCase(IUserDataAccess userDataAccess, IPasswordService passwordService)
    {
        this.userDataAccess = userDataAccess;
        this.passwordService = passwordService;
    }

    public Task<Result<UserSignUpOutput>> Execute(UserSignUpInput input)
    {
        // Validate user
        // Chech e-mail is available
        // Hash password
        // Create user with input + passwordHash

        Result<UserSignUpOutput> result = Result<UserSignUpOutput>.Successed();

        return Task.FromResult(result);
    }
}
