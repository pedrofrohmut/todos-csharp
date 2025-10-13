namespace Todos.WebApi.Controllers;

public class ControllerUtils
{
    public static async Task WriteExceptionResponse(HttpContext httpContext, Exception e)
    {
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsync($"Server Error: {e.Message}" );
    }
}
