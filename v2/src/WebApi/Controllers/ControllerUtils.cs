using Todos.Core.Errors;

namespace Todos.WebApi.Controllers;

public class ControllerUtils
{
    public static async Task WriteExceptionResponse(string methodName, HttpContext httpContext, Exception e)
    {
        Console.WriteLine($"ERROR [{methodName}]: {e.Message}\n{e.StackTrace}");
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsync($"Server Error: {e.Message}" );
    }

    public static async Task WriteErrorNotMappedResponse(HttpContext httpContext, ResultError? err = null)
    {
        httpContext.Response.StatusCode = 500;
        if (err != null) {
            await httpContext.Response.WriteAsync("Server Error: " + err.Message);
        } else {
            await httpContext.Response.WriteAsync("Server Error: Result Error returned is not mapped.");
        }
    }

    public static string? GetAuthToken(HttpRequest request)
    {
        return request.Headers.Authorization.ToString().Split(" ")[1];
    }
}
