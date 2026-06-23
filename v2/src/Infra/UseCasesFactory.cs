using Todos.Core.UseCases.Users;
using Todos.Core.UseCases.Todos;
using Todos.Infra.Handlers.Commands;
using Todos.Infra.Handlers.Queries;
using Todos.Infra.Services;
using System.Data;
using DotNetEnv;
using System.Text;

namespace Todos.Infra;

public static class UseCasesFactory
{
    private static readonly byte[] secretKey;

    static UseCasesFactory()
    {
        Env.Load("../../.env"); // Load the .env file

        string? envSecret = System.Environment.GetEnvironmentVariable("JWT_SECRET");
        if (String.IsNullOrWhiteSpace(envSecret)) {
            throw new ArgumentNullException(
                "No JWT_SECRET found in the enviroment when looking for it in the .env file.");
        }

        secretKey = Convert.FromBase64String(envSecret);
    }

    public static UserSignUpUseCase GetUserSignUpUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        // TODO: add the write and read dbConnections
        // TODO: Check if is needed to open the connection

        var userQueryHandler = new UserQueryHandler(writeConnection);
        var userCommandHandler = new UserCommandHandler(writeConnection, readConnection);
        var passwordService = new PasswordService();
        return new UserSignUpUseCase(userQueryHandler, userCommandHandler, passwordService);
    }

    public static UserSignInUseCase GetUserSignInUseCase(IDbConnection readConnection)
    {
        // TODO: add the read dbConnections
        var userQueryHandler = new UserQueryHandler(readConnection);
        var passwordService = new PasswordService();
        var authTokenService = new AuthTokenService(secretKey);
        return new UserSignInUseCase(userQueryHandler, passwordService, authTokenService);
    }

    public static VerifyAuthTokenUseCase GetVerifyAuthTokenUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnections
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        return new VerifyAuthTokenUseCase(authTokenService, userQueryHandler);
    }

    public static CreateTodoUseCase GetCreateTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        // TODO: add the write and read dbConnections
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new CreateTodoUseCase(authTokenService, userQueryHandler, todoCommandHandler);
    }

    public static DeleteTodoUseCase GetDeleteTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new DeleteTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }

    public static FindTodoByIdUseCase GetFindTodoByIdUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnection
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        return new FindTodoByIdUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static FindAllTodosUseCase GetFindAllTodosUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnection
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        return new FindAllTodosUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static UpdateTodoUseCase GetUpdateTodoUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        var authTokenService = new AuthTokenService(secretKey);
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        var todoCommandHandler = new TodoCommandHandler(writeConnection, readConnection);
        return new UpdateTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }
}
