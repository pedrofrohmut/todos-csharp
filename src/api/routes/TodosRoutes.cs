namespace Todos.Api.Routes;

public static class TodosRoutes
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("api/todos/clearcomplete",        () => "Clear Complete Todos");

        app.MapPost  ("api/todos",                      () => "Create Todo");

        app.MapDelete("api/todos/{todoId}",             () => "Delete Todos");

        app.MapGet   ("api/todos/{todoId}",             () => "Find Todo By Id");
        
        app.MapGet   ("api/todos/task/{taskId}",        () => "Find Todos By Task Id");

        app.MapPut   ("api/todos/complete/{todoId}",    () => "Set todo as complete");

        app.MapPut   ("api/todos/notcomplete/{todoId}", () => "Set todo as not complete");

        app.MapPut   ("api/todos/{todoId}",             () => "Update Todo");
    }
}
