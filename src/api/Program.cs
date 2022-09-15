using Todos.Api.Routes;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Routes
UsersRoutes.Map(app);
TasksRoutes.Map(app);
TodosRoutes.Map(app);

app.Run();
