using Microsoft.AspNetCore.Mvc;

using Todos.Core.WebIO;
using Todos.Core.Dtos;
using Todos.Core.DataAccess;
using Todos.Core.UseCases.Users;
using Todos.DataAccess;
using Todos.Services;
using System.Data;

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
            Console.WriteLine("ERROR => UsersController::SignUp: " + e.Message);
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
            Console.WriteLine("ERROR => UsersController::SignIn: " + e.Message);
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
        var authUserId = "";
        try {
            var token = Request.Headers.Authorization.ToString().Split(" ")[1];
            var decoded = tokenService.DecodeToken(token);
            authUserId = decoded.UserId;
        } catch (Exception e) {
            if (e is ArgumentException) {
                return new ObjectResult(false) { StatusCode = 200 };
            }
            throw;
        }
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var userDataAccess = new UserDataAccess(connection);
        var verifyUserUseCase = new VerifyUserUseCase(userDataAccess);
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var response = new UsersWebIO().VerifyUser(verifyUserUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }
}
