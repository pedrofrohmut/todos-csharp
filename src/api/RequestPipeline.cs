namespace Todos.Api;

public static class RequestPipeline
{
    public static void Configure(WebApplication app)
    {
        app.UseCors(builder => 
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.MapControllers();
    }
}
