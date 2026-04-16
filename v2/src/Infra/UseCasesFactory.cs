using Todos.Core.UseCases.Users;
using Todos.Core.UseCases.Todos;
using Todos.Infra.Handlers.Commands;
using Todos.Infra.Handlers.Queries;
using Todos.Infra.Services;
using System.Data;

namespace Todos.Infra;

public static class UseCasesFactory
{
    public static UserSignUpUseCase GetUserSignUpUseCase(IDbConnection writeConnection, IDbConnection readConnection)
    {
        // TODO: add the write and read dbConnections
        // TODO: Check if is needed to open the connection

        var userQueryHandler = new UserQueryHandler(writeConnection);
        var userCommandHandler = new UserCommandHandler(writeConnection, readConnection);
        var passwordService = new PasswordService();
        return new UserSignUpUseCase(userQueryHandler, userCommandHandler, passwordService);
    }

    public static UserSignInUseCase GetUserSignInUseCase(IDbConnection writeConnection)
    {
        // TODO: add the read dbConnections
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var passwordService = new PasswordService();
        return new UserSignInUseCase(userQueryHandler, passwordService);
    }

    public static VerifyAuthTokenUseCase GetVerifyAuthTokenUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnections
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        return new VerifyAuthTokenUseCase(authTokenService, userQueryHandler);
    }

    public static CreateTodoUseCase GetCreateTodoUseCase(IDbConnection writeConnection)
    {
        // TODO: add the write and read dbConnections
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoCommandHandler = new TodoCommandHandler();
        return new CreateTodoUseCase(authTokenService, userQueryHandler, todoCommandHandler);
    }

    public static DeleteTodoUseCase GetDeleteTodoUseCase(IDbConnection writeConnection)
    {
        // TODO: add dbConnections
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        var todoCommandHandler = new TodoCommandHandler();
        return new DeleteTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }

    public static FindTodoByIdUseCase GetFindTodoByIdUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnection
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        return new FindTodoByIdUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static FindAllTodosUseCase GetFindAllTodosUseCase(IDbConnection writeConnection)
    {
        // TODO: add read dbConnection
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        return new FindAllTodosUseCase(authTokenService, userQueryHandler, todoQueryHandler);
    }

    public static UpdateTodoUseCase GetUpdateTodoUseCase(IDbConnection writeConnection)
    {
        // TODO: add the write and read dbConnections
        var authTokenService = new AuthTokenService();
        var userQueryHandler = new UserQueryHandler(writeConnection);
        var todoQueryHandler = new TodoQueryHandler();
        var todoCommandHandler = new TodoCommandHandler();
        return new UpdateTodoUseCase(authTokenService, userQueryHandler, todoQueryHandler, todoCommandHandler);
    }
}
