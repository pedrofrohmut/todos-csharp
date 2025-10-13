using Todos.Core.UseCases.Users;
using Todos.Infra.DataAccess;
using Todos.Infra.Services;

namespace Todos.Infra;

public static class UseCasesFactory
{
    public static UserSignUpUseCase GetUserSignUpUseCase()
    {
        // TODO: add the write and read dbConnections, add the userDataAccess and the passwordService
        var userDataAccess = new UserDataAccess();
        var passwordService = new PasswordService();
        return new UserSignUpUseCase(userDataAccess, passwordService);
    }
}
