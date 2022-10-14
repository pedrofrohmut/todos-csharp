using System.Data;
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
public class TodosController : Controller
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

    [HttpGet("task/{id}")]
    public ActionResult Find(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodosByTaskIdUseCase =
            new FindTodosByTaskIdUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        try {
            var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
            var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
            var response = new TodosWebIO().FindByTaskId(findTodosByTaskIdUseCase, webRequest);
            var responseValue = response.Message != "" ? response.Message : response.Body;
            return new ObjectResult(responseValue) { StatusCode = response.Status };
        } catch (InvalidRequestAuthException e) {
            return new ObjectResult(e.Message) { StatusCode = 401 };
        } catch (Exception e) {
            // This catch block should only catch unwanted exceptions
            Console.WriteLine("ERROR => TasksController::Create: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return new ObjectResult("Server Error") { StatusCode = 500 };
        }
    }

    [HttpGet("{id}")]
    public ActionResult FindById(string id)
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodoByIdUseCase = new FindTodoByIdUseCase(userDataAccess, todoDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var authUserId = ControllerUtils.GetUserIdFromRequest(Request, tokenService);
            var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
            var response = new TodosWebIO().FindById(findTodoByIdUseCase, webRequest);
            var responseValue = response.Message != "" ? response.Message : response.Body;
            return new ObjectResult(responseValue) { StatusCode = response.Status };
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

    [HttpPatch("setdone/{id}")]
    public ActionResult SetDone(string id)
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoDoneUseCase = new SetTodoDoneUseCase(userDataAccess, todoDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var authUserId = ControllerUtils.GetUserIdFromRequest(Request, tokenService);
            var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
            var response = new TodosWebIO().SetDone(setTodoDoneUseCase, webRequest);
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

    [HttpPatch("setnotdone/{id}")]
    public ActionResult SetNotDone(string id)
    {
        var tokenService = new TokenService(this.configuration["jwtSecret"]);
        var connection = this.connectionManager.GetConnection(this.configuration);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoNotDoneUseCase = new SetTodoNotDoneUseCase(userDataAccess, todoDataAccess);
        try {
            this.connectionManager.OpenConnection(connection);
            var authUserId = ControllerUtils.GetUserIdFromRequest(Request, tokenService);
            var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
            var response = new TodosWebIO().SetNotDone(setTodoNotDoneUseCase, webRequest);
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
