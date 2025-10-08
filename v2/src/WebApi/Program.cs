var builder = WebApplication.CreateBuilder(args);
// Add Dbconnection, cors and controllers

var app = builder.Build();
// Configure: UseCors and MapControllers
// Middlewares: TimeIt, ExceptionHandler, Authorization, DatabaseConnection

app.Run();
