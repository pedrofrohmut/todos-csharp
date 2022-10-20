using System.Data;
using Microsoft.AspNetCore.Diagnostics;
using Todos.Core.Exceptions;
using Todos.DataAccess;
using Todos.Services;

namespace Todos.Api;

// - [ ] (17/10/2022) Refactor Exception handler to return a 401 response for auth exceptions

// - [ ] (15/10/2022) [TODO] Refactor the RequestPipeline class => replace comments with private methods 
// and remove extra private methods to center related code in the same place

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
        ExceptionMiddleware(app);

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

    private static void ExceptionMiddleware(WebApplication app)
    {
        app.UseExceptionHandler(app => {
            app.Run(async ctx => {
                var exceptionFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionFeature != null) {
                    var error = exceptionFeature.Error;
                    if (error is InvalidRequestAuthException) {
                        ctx.Response.StatusCode = 401;
                        await ctx.Response.WriteAsync("Unauthorized: " + error.Message);
                        return;
                    }
                    Console.WriteLine("\nError => " + error.Message);
                    Console.WriteLine("StackTrace => \n" + error.StackTrace + "\n");
                    ctx.Response.StatusCode = 500;
                    await ctx.Response.WriteAsync("Server Error !!!");
                }
            });
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
