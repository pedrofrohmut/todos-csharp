using Todos.Core.UseCases.Users;
using Todos.Core.UseCases.Todos;
using Todos.Infra.Handlers.Commands;
using Todos.Infra.Handlers.Queries;
using Todos.Infra.Services;
using System.Data;
using System.Text;

namespace Todos.Infra;

public static class UseCasesFactory
{
    private static readonly byte[] secretKey;

    static UseCasesFactory()
    {
        string? envSecret = System.Environment.GetEnvironmentVariable("JWT_SECRET");
        if (String.IsNullOrWhiteSpace(envSecret)) {
            throw new ArgumentNullException(
                "No JWT_SECRET found in the enviroment when looking for it in the .env file.");
        }

        secretKey = Convert.FromBase64String(envSecret);
    }

    public static UserSignUpUseCase GetUserSignUpUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var userQueryHandler = new UserQueryHandler(readConnection);
        var userCommandHandler = new UserCommandHandler(writeConnection, readConnection);
        var passwordService = new PasswordService();
        return new UserSignUpUseCase(userQueryHandler, userCommandHandler, passwordService);
    }

    public static UserSignInUseCase GetUserSignInUseCase(IDbConnection readConnection)
    {
        var userQueryHandler = new UserQueryHandler(readConnection);
        var passwordService = new PasswordService();
        var authTokenService = new AuthTokenService(secretKey);
        return new UserSignInUseCase(userQueryHandler, passwordService, authTokenService);
    }

    public static VerifyAuthTokenUseCase GetVerifyAuthTokenUseCase(IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        return new VerifyAuthTokenUseCase(authTokenService, userQueryHandler);
    }

    public static CreateTodoUseCase GetCreateTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new CreateTodoUseCase(authTokenService, userQueryHandler, todoCommandHandler);
    }

    public static DeleteTodoUseCase GetDeleteTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        var todoQueryHandler = new TodoQueryHandler(readConnection);
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new DeleteTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }

    public static FindTodoByIdUseCase GetFindTodoByIdUseCase(IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        var todoQueryHandler = new TodoQueryHandler(readConnection);
        return new FindTodoByIdUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static FindAllTodosUseCase GetFindAllTodosUseCase(IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        var todoQueryHandler = new TodoQueryHandler(readConnection);
        return new FindAllTodosUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static UpdateTodoUseCase GetUpdateTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(readConnection);
        var todoQueryHandler = new TodoQueryHandler(readConnection);
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new UpdateTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }
}
