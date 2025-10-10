using Microsoft.AspNetCore.Mvc;

namespace Todos.WebApi.Controllers;

[Route("api/v2/users")]
public class UsersController : ControllerBase
{
    [HttpPost("signup")]
    public async Task SignUp()
    {
        HttpContext.Response.StatusCode = 201;
        await HttpContext.Response.WriteAsync("Sign Up");
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
