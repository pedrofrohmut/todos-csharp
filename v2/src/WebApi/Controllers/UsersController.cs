using Microsoft.AspNetCore.Mvc;
using Todos.Core.UseCases.Users;
using Todos.Infra;
using Todos.Core.Errors;

namespace Todos.WebApi.Controllers;

public readonly struct SignUpBody
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}

[Route("api/v2/users")]
public class UsersController : ControllerBase
{
    [HttpPost("signup")]
    public async Task SignUp(SignUpBody body)
    {
        try {
            var useCase = UseCasesFactory.GetUserSignUpUseCase();
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

            HttpContext.Response.StatusCode = 500;
            await HttpContext.Response.WriteAsync("Server Error: Result Error returned is not mapped.");
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
        }
    }

    [HttpPost("signin")]
    public async Task SignIn()
    {
        HttpContext.Response.StatusCode = 200;
        await HttpContext.Response.WriteAsync("Sign In");
    }

    [HttpGet("verify")]
    public async Task Verify()
    {
        HttpContext.Response.StatusCode = 200;
        await HttpContext.Response.WriteAsync("Verify");
    }
}
