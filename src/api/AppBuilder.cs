using Todos.Core.DataAccess;
using Todos.DataAccess;

namespace Todos.Api;

public static class AppBuilder
{
    public static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
        builder.Services.AddCors();
        builder.Services.AddControllers();
    }
}
