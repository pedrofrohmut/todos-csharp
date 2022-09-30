using Microsoft.AspNetCore.Mvc;

using Todos.Core.WebIO;
using Todos.Core.Dtos;
using Todos.Core.DataAccess;
using Todos.Core.UseCases.Tasks;
using Todos.DataAccess;
using Todos.Services;

namespace Todos.Api.Controllers;

[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IConnectionManager connectionManager;

    public TasksController(
        IConfiguration configuration,
        IConnectionManager connectionManager)
    {
        this.configuration = configuration;
        this.connectionManager = connectionManager;
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateTaskDto newTask)
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var authUserId = "";
        try {
            authUserId = ControllerUtils.GetUserIdFromToken(Request, tokenService);
        } catch (ArgumentException) {
            return new ObjectResult("Not authorized") { StatusCode = 401 };
        }
        var webRequest = new WebRequestDto() { Body = newTask, AuthUserId = authUserId };
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var createTaskUseCase = new CreateTaskUseCase(userDataAccess, taskDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var response = new TasksWebIO().Create(createTaskUseCase, webRequest);
            return new ObjectResult(response.Message) { StatusCode = response.Status };
        } catch (Exception e) {
            // This catch block should only catch unwanted exceptions
            Console.WriteLine("ERROR => TasksController::Create: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return new ObjectResult("Server Error") { StatusCode = 500 };
        } finally {
            this.connectionManager.CloseConnection(connection);
        }
    }
}
