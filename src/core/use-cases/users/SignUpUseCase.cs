using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Services;

namespace Todos.Core.UseCases.Users;

public class SignUpUseCase : ISignUpUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly IPasswordService passwordService;

    public SignUpUseCase(IUserDataAccess userDataAccess, IPasswordService passwordService)
    {
        this.userDataAccess = userDataAccess;
        this.passwordService = passwordService;
    }

    public void Execute(CreateUserDto newUser)
    {
        throw new NotImplementedException();
    }
}
