using Microsoft.AspNetCore.Mvc;
using Todos.Core.UseCases.Users;
using Todos.Infra;
using Todos.Core.Errors;
using Todos.Core.Utils;

namespace Todos.WebApi.Controllers;

public record SignUpBody(
    string Name,
    string Email,
    string Password
);

public readonly struct SignInBody
{
    public string Email { get; init; }
    public string Password { get; init; }
}

[Route("api/v2/users")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration configuration;

    public UsersController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpPost("signup")]
    public async Task SignUp([FromBody] SignUpBody body)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);
        var readConnection = DbConnectionManager.GetReadConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetUserSignUpUseCase(writeConnection, readConnection);
            var input = new UserSignUpInput {
                Name = body.Name,
                Email = body.Email,
                Password = body.Password,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 201;
                await HttpContext.Response.WriteAsync("Sign Up: User created successfully.");
                return;
            }

            if (result.Error is InvalidUserError || result.Error is EmailAlreadyTakenError) {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext, result.Error);
        } catch (Exception e) {
            Console.WriteLine($"ERROR [SignUp]: {e.Message}\n{e.StackTrace}");
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
            DbConnectionManager.CloseConnection(readConnection);
        }
    }

    [HttpPost("signin")]
    public async Task SignIn(SignInBody body)
    {
        var useCase = UseCasesFactory.GetUserSignInUseCase();
        var input = new UserSignInInput {
            Email = body.Email,
            Password = body.Password,
        };

        Result<UserSignInOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

        if (result.IsSuccess) {
            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(result.Payload);
            return;
        }

        if (result.Error is InvalidUserError || result.Error is PasswordMatchError) {
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        if (result.Error is UserNotFoundError) {
            HttpContext.Response.StatusCode = 404;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
    }

    /* Just to check if the Authentication token is valid or not */
    [HttpGet("verify")]
    public async Task Verify()
    {
        var token = ControllerUtils.GetAuthToken(Request);
        var useCase = UseCasesFactory.GetVerifyAuthTokenUseCase();
        var input = new VerifyAuthTokenInput { Token = token };

        Result<VerifyAuthTokenOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

        if (result.IsSuccess) {
            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(result.Payload);
            return;
        }

        if (result.Error is InvalidTokenError) {
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        if (result.Error is UserNotFoundError) {
            HttpContext.Response.StatusCode = 404;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
    }
}
