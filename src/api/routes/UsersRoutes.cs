namespace Todos.Api.Routes;

public static class UsersRoutes
{
    public static void Map(WebApplication app)
    {
        app.MapPost("api/users/",       () => "Sign Up");

        app.MapPost("api/users/signin", () => "Sign In");

        app.MapGet ("api/users/verify", () => "Verify");
    }
}
