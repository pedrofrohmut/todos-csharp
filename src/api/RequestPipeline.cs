using System.Data;
using Microsoft.AspNetCore.Diagnostics;
using Todos.DataAccess;
using Todos.Services;

namespace Todos.Api;

// TODO: Refactor replace comments with private methods and remove extra private methods to center
// related code in the same place
public static class RequestPipeline
{
    public static void Configure(WebApplication app)
    {
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.MapControllers();
    }

    public static void ExecuteMiddlewares(WebApplication app)
    {
        // Exception Middleware
        app.UseExceptionHandler(app => {
            app.Run(async ctx => {
                var error = ctx.Features.Get<IExceptionHandlerPathFeature>()!.Error;
                Console.WriteLine("\nError => " + error.Message);
                Console.WriteLine("StackTrace => \n" + error.StackTrace + "\n");
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsync("Server Error !!!");
            });
        });
        // Authorization Headers
        app.Use(async (ctx, next) => {
            if (ctx.Request.Path != "/api/users/verify") {
                Auth_DoBefore(ctx, app);
            }
            await next.Invoke(ctx);
        });
        // Create, Open and Close Database Connection
        app.Use(async (ctx, next) => {
            Connect_DoBefore(ctx, app);
            await next.Invoke(ctx);
            Connect_DoAfter(ctx);
        });
    }

    private static string GetUserIdFromToken(HttpContext ctx, WebApplication app)
    {
        var token = ctx.Request.Headers.Authorization.ToString().Split(" ")[1];
        var tokenService = new TokenService(app.Configuration["jwtSecret"]);
        var decodedToken = tokenService.DecodeToken(token);
        return decodedToken.UserId;
    }

    private static void Auth_DoBefore(HttpContext ctx, WebApplication app)
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

    private static void Connect_DoBefore(HttpContext ctx, WebApplication app)
    {
        var manager = new ConnectionManager();
        var connection = manager.GetConnection(app.Configuration);
        ctx.Items.Add("connection", connection);
        manager.OpenConnection(connection);
    }

    private static void Connect_DoAfter(HttpContext ctx)
    {
        var manager = new ConnectionManager();
        var connection = ctx.Items["connection"];
        if (connection != null) {
            manager.CloseConnection((IDbConnection) connection);
        }
    }
}
