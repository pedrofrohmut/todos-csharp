using Microsoft.AspNetCore.Mvc;

namespace Todos.WebApi.Controllers;

public class HealthController : ControllerBase
{
    [Route("health")]
    public string CheckHealth()
    {
        return "Todos-CSharp Server is online";
    }
}
