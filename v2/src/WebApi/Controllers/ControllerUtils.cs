namespace Todos.WebApi.Controllers;

public class ControllerUtils
{
    public static async Task WriteExceptionResponse(HttpContext httpContext, Exception e)
    {
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsync($"Server Error: {e.Message}" );
    }

    public static async Task WriteErrorNotMappedResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsync("Server Error: Result Error returned is not mapped.");
    }

    public static string? GetAuthToken(HttpRequest request)
    {
        return request.Headers.Authorization.ToString().Split(" ")[1];
    }
}
