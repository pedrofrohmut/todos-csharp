using Microsoft.AspNetCore.Mvc;

namespace Todos.WebApi.Controllers;

[Route("api/v2/")]
public class BaseController : ControllerBase
{
    [Route("{**catchAll}")]
    public async Task CatchAll()
    {
        HttpContext.Response.StatusCode = 404;
        await HttpContext.Response.WriteAsync("Route not mapped, misspelled or with wrong method.");
    }
}
