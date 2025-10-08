using System.Data;
using Microsoft.AspNetCore.Diagnostics;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Todos.DataAccess;
using Todos.Services;

namespace Todos.Api;

public static class RequestPipeline
{
    public static void Configure(WebApplication app)
    {
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.MapControllers();
    }

    public static void ExecuteMiddlewares(WebApplication app)
    {
        TimeItMiddleware(app);
        ExceptionMiddleware(app);
        AuthorizationMiddleware(app);
        DatabaseConnectionMiddleware(app);
    }

    private static void TimeItMiddleware(WebApplication app)
    {
        app.Use(async (ctx, next) => {
            var start = DateTime.UtcNow;
            await next.Invoke(ctx);
            var total = (DateTime.UtcNow - start).TotalMilliseconds;
            Console.WriteLine($"Time on request '{ctx.Request.Path}' is {total}");
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

    private static void AuthorizationMiddleware(WebApplication app)
    {
        // hasAuthHeaders ? validAuthUserId : ""
        app.Use(async (ctx, next) => {
            if (ctx.Request.Path != "/api/users/verify") {
                var auth = ctx.Request.Headers.Authorization;
                if (String.IsNullOrWhiteSpace(auth)) {
                    ctx.Items.Add("authUserId", "");
                } else {
                    var authUserId = GetUserIdFromToken(ctx, app);
                    ctx.Items.Add("authUserId", authUserId);
                }
            }
            await next.Invoke(ctx);
        });
    }

    private static string GetUserIdFromToken(HttpContext ctx, WebApplication app)
    {
        try {
            var token = ctx.Request.Headers.Authorization.ToString().Split(" ")[1];
            var tokenService = new TokenService(app.Configuration["jwtSecret"]);
            var userId = tokenService.DecodeToken(token).UserId;
            User.ValidateId(userId);
            return userId;
        } catch (Exception e) {
            if (e is ArgumentException) {
                throw new InvalidRequestAuthException();
            }
            throw new InvalidRequestAuthException(e.Message);
        }
    }

    private static void DatabaseConnectionMiddleware(WebApplication app)
    {
        // Create, Open and Close Database Connection
        app.Use(async (ctx, next) => {
            //Before
            var manager = new ConnectionManager();
            var connection = manager.GetConnection(app.Configuration);
            manager.OpenConnection(connection);
            ctx.Items.Add("connection", connection);
            await next.Invoke(ctx);
            // After
            if (connection != null) {
                manager.CloseConnection((IDbConnection) connection);
            }
        });
    }
}
