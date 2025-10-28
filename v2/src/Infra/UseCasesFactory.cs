using Todos.Core.UseCases.Users;
using Todos.Core.UseCases.Todos;
using Todos.Infra.Handlers.Commands;
using Todos.Infra.Handlers.Queries;
using Todos.Infra.Services;

namespace Todos.Infra;

public static class UseCasesFactory
{
    public static UserSignUpUseCase GetUserSignUpUseCase()
    {
        // TODO: add the write and read dbConnections
        var userQueryHandler = new UserQueryHandler();
        var userCommandHandler = new UserCommandHandler();
        var passwordService = new PasswordService();
        return new UserSignUpUseCase(userQueryHandler, userCommandHandler, passwordService);
    }

    public static UserSignInUseCase GetUserSignInUseCase()
    {
        // TODO: add the read dbConnections
        var userQueryHandler = new UserQueryHandler();
        var passwordService = new PasswordService();
        return new UserSignInUseCase(userQueryHandler, passwordService);
    }

    public static VerifyAuthTokenUseCase GetVerifyAuthTokenUseCase()
    {
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler();
        return new VerifyAuthTokenUseCase(authTokenService, userQueryHandler);
    }

    public static CreateTodoUseCase GetCreateTodoUseCase()
    {
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler();
        var todoCommandHandler = new TodoCommandHandler();
        return new CreateTodoUseCase(authTokenService, userQueryHandler, todoCommandHandler);
    }
}
