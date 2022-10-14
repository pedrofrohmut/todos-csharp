using System.Data;
using Todos.DataAccess;
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
            DoBeforeAuth(ctx, app);
            await next.Invoke(ctx);
        });

        app.Use(async (ctx, next) => {
            DoBeforeConnect(ctx, app);
            await next.Invoke(ctx);
            DoAfterConnect(ctx);
        });
    }

    private static string GetUserIdFromToken(HttpContext ctx, WebApplication app)
    {
        var token = ctx.Request.Headers.Authorization.ToString().Split(" ")[1];
        var tokenService = new TokenService(app.Configuration["jwtSecret"]);
        var decodedToken = tokenService.DecodeToken(token);
        return decodedToken.UserId;
    }

    private static void DoBeforeAuth(HttpContext ctx, WebApplication app)
    {
        var auth = ctx.Request.Headers.Authorization;
        if (String.IsNullOrWhiteSpace(auth)) {
            // Set authUserId to empty string if don't have Authorization Headers
            ctx.Items.Add("authUserId", "");
        } else {
            var authUserId = GetUserIdFromToken(ctx, app);
            ctx.Items.Add("authUserId", authUserId);
        }
    }

    private static void DoBeforeConnect(HttpContext ctx, WebApplication app)
    {
        var manager = new ConnectionManager();
        var connection = manager.GetConnection(app.Configuration);
        ctx.Items.Add("connection", connection);
        manager.OpenConnection(connection);
    }

    private static void DoAfterConnect(HttpContext ctx)
    {
        var manager = new ConnectionManager();
        var connection = ctx.Items["connection"];
        if (connection != null) {
            manager.CloseConnection((IDbConnection) connection);
        }
    }
}
