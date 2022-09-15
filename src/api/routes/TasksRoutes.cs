namespace Todos.Api.Routes;

public static class TasksRoutes
{
    public static void Map(WebApplication app)
    {
        app.MapPost  ("api/tasks/",              () => "Create Task");

        app.MapDelete("api/tasks/{taskId}",      () => "Delete Task");

        app.MapGet   ("api/tasks/{taskId}",      () => "Find Task By Id");

        app.MapGet   ("api/tasks/user/{userId}", () => "Find Task By User Id");

        app.MapPut   ("api/tasks/{taskId}", () => "Find Task By User Id");
    }
}
