using Microsoft.AspNetCore.Mvc;
using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Exceptions;
using Todos.Core.UseCases.Todos;
using Todos.Core.WebIO;
using Todos.DataAccess;
using Todos.Services;

namespace Todos.Api.Controllers;

[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IConnectionManager connectionManager;

    public TodosController(
        IConfiguration configuration,
        IConnectionManager connectionManager)
    {
        this.configuration = configuration;
        this.connectionManager = connectionManager;
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateTodoDto newTodo)
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var createTodoUseCase =
            new CreateTodoUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var authUserId = ControllerUtils.GetUserIdFromRequest(Request, tokenService);
            var webRequest = new WebRequestDto() { Body = newTodo, AuthUserId = authUserId };
            var response = new TodosWebIO().Create(createTodoUseCase, webRequest);
            return new ObjectResult(response.Message) { StatusCode = response.Status };
        } catch (InvalidRequestAuthException e) {
            return new ObjectResult(e.Message) { StatusCode = 401 };
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
