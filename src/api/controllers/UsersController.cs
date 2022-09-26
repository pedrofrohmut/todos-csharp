using Microsoft.AspNetCore.Mvc;
using Todos.Core.WebIO;
using Todos.Core.Dtos;
using Todos.Core.DataAccess;
using Todos.Core.UseCases.Users;
using Todos.DataAccess.Users;
using Todos.Services;

namespace Todos.Api.Controllers;

[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IConnectionManager connectionManager;

    public UserController(IConfiguration configuration, IConnectionManager connectionManager)
    {
        this.configuration = configuration;
        this.connectionManager = connectionManager;
    }

    [HttpPost]
    public ActionResult SignUp([FromBody] CreateUserDto newUser)
    {
        var webRequest = new WebRequestDto() { Body = newUser };
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var passwordService = new PasswordService();
        var signUpUseCase = new SignUpUseCase(userDataAccess, passwordService);
        try {
            this.connectionManager.OpenConnection(connection);
            var response = new UsersWebIO().SignUpUser(signUpUseCase, webRequest);
            return new ObjectResult(response.Message) { StatusCode = response.Status };
        } catch (Exception e) {
            // This catch block should only catch unwanted exceptions
            Console.WriteLine("ERROR => UserApiController::SignUp: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return new ObjectResult("Server Error") { StatusCode = 500 };
        } finally {
            this.connectionManager.CloseConnection(connection);
        }
    }

    [HttpPost("signin")]
    public ActionResult SignIn([FromBody] UserCredentialsDto credentials)
    {
        var webRequest = new WebRequestDto() { Body = credentials };
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var passwordService = new PasswordService();
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var signInUseCase = new SignInUseCase(userDataAccess, passwordService, tokenService);
        try {
            this.connectionManager.OpenConnection(connection);
            var response = new UsersWebIO().SignInUser(signInUseCase, webRequest);
            var responseValue = response.Message != "" ? response.Message : response.Body;
            return new ObjectResult(responseValue) { StatusCode = response.Status };
        } catch (Exception e) {
            // This catch block should only catch unwanted exceptions
            Console.WriteLine("ERROR => UserApiController::SignIn: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return new ObjectResult("Server Error") { StatusCode = 500 };
        } finally {
            this.connectionManager.CloseConnection(connection);
        }
    }

    [HttpGet("verify")]
    public ActionResult Verify()
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        string authUserId = "";
        try {
            authUserId = ControllerUtils.GetUserIdFromToken(Request, tokenService);
        } catch (ArgumentException) {
            return new ObjectResult(false) { StatusCode = 200 };
        }
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var verifyUserUseCase = new VerifyUserUseCase(userDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var response = new UsersWebIO().VerifyUser(verifyUserUseCase, webRequest);
            var responseValue = response.Message != "" ? response.Message : response.Body;
            return new ObjectResult(responseValue) { StatusCode = response.Status };
        } catch (Exception e) {
            // This catch block should only catch unwanted exceptions
            Console.WriteLine("ERROR => UserApiController::SignIn: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return new ObjectResult("Server Error") { StatusCode = 500 };
        } finally {
            this.connectionManager.CloseConnection(connection);
        }
    }
}
