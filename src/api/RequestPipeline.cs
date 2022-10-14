using Todos.Services;

namespace Todos.Api;

public static class RequestPipeline
{
    public static void Configure(WebApplication app)
    {
        app.UseCors(builder => 
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.MapControllers();
    }

    public static void ExecuteMiddlewares(WebApplication app)
    {
        app.Use(async (ctx, next) => {
            var auth = ctx.Request.Headers.Authorization;
            if (String.IsNullOrWhiteSpace(auth)) {
                // Set authUserId to empty string if don't have Authorization Headers
                ctx.Items.Add("authUserId", "");
            } else {
                var authUserId = GetUserIdFromToken(ctx, app);
                ctx.Items.Add("authUserId", authUserId);
            }
            await next.Invoke(ctx);
        });
    }

    private static string GetUserIdFromToken(HttpContext ctx, WebApplication app)
    {
        var token = ctx.Request.Headers.Authorization.ToString().Split(" ")[1];
        var tokenService = new TokenService(app.Configuration["jwtSecret"]);
        var decodedToken = tokenService.DecodeToken(token);
        return decodedToken.UserId;
    }
}
