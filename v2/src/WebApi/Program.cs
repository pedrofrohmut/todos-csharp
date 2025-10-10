var builder = WebApplication.CreateBuilder(args);
BuilderAddServices(builder);

var app = builder.Build();
AppConfigure(app);
AppUseMiddlewares(app);

app.Run();

static void BuilderAddServices(WebApplicationBuilder builder)
{
    builder.Services.AddCors();
    builder.Services.AddControllers();
}

static void AppConfigure(WebApplication app)
{
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    app.MapControllers();
}

static void AppUseMiddlewares(WebApplication app)
{
    TimeItMiddleware(app);
}

static void TimeItMiddleware(WebApplication app)
{
    app.Use(async (ctx, next) => {
        var start = DateTime.UtcNow;
        await next.Invoke(ctx);
        var elapsed = DateTime.UtcNow - start;
        var elapsedInMills = elapsed.TotalMilliseconds;
        Console.WriteLine($"INFO: Took {elapsedInMills} ms to process the {ctx.Request.Path} request.");
    });
}
