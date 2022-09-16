using Microsoft.AspNetCore.Mvc;
using Todos.Core.Controllers;
using Todos.Core.Dtos;

namespace Todos.Api.Controllers;

[Route("api/users")]
public class UserApiController : ControllerBase
{
    [HttpPost]
    public ActionResult SignUp(CreateUserDto newUser)
    {
        var adaptedRequest = new AdaptedRequest() {
            Body = newUser
        };
        var controller = new UsersController();
        var response = controller.SignUpUser(adaptedRequest);
        return new ObjectResult(response.Msg) { StatusCode = response.Status };
    }

    [HttpPost("signin")]
    public ActionResult SignIn(UserCredentialsDto credentials)
    {
        var adaptedRequest = new AdaptedRequest() {
            Body = credentials
        };
        var controller = new UsersController();
        var res = controller.SignInUser(adaptedRequest);
        return new ObjectResult(res.Msg == "" ? res.Body : res.Msg) { 
            StatusCode = res.Status 
        };
    }

    [HttpGet("verify")]
    public ActionResult Verify()
    {
        if (Request.Headers.TryGetValue("Authorization", out var auth)) {
            var token = auth.ToString().Split(" ")[1];
        }        
        // TODO: decode token to get authUserId
        var authUserId = "userId1234";
        var adaptedRequest = new AdaptedRequest() {
            AuthUserId = authUserId
        };
        var controller = new UsersController();
        var res = controller.Verify(adaptedRequest);
        return new ObjectResult(res.Msg == "" ? res.Body : res.Msg) {
            StatusCode = res.Status
        };
    }
}
