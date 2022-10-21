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
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var createTodoUseCase = new CreateTodoUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Body = newTodo, AuthUserId = authUserId };
        var response = new TodosWebIO().Create(createTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("task/{id}")]
    public ActionResult Find(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodosByTaskIdUseCase =
            new FindTodosByTaskIdUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().FindByTaskId(findTodosByTaskIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("{id}")]
    public ActionResult FindById(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodoByIdUseCase = new FindTodoByIdUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().FindById(findTodoByIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPatch("setdone/{id}")]
    public ActionResult SetDone(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoDoneUseCase = new SetTodoDoneUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().SetDone(setTodoDoneUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPatch("setnotdone/{id}")]
    public ActionResult SetNotDone(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoNotDoneUseCase = new SetTodoNotDoneUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().SetNotDone(setTodoNotDoneUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPut("{id}")]
    public ActionResult Update(string id, [FromBody] UpdateTodoDto updatedTodo)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var updateTodoUseCase = new UpdateTodoUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, Body = updatedTodo, AuthUserId = authUserId };
        var response = new TodosWebIO().Update(updateTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteTodoUseCase = new DeleteTodoUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().Delete(deleteTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("done")]
    public ActionResult DeleteDone()
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteDoneTodosUseCase = new DeleteDoneTodosUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var response = new TodosWebIO().DeleteDone(deleteDoneTodosUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("done/task/{id}")]
    public ActionResult DeleteDoneByTaskId(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteDoneTodosByTaskIdUseCase =
            new DeleteDoneTodosByTaskIdUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = new TodosWebIO().DeleteDoneByTaskId(deleteDoneTodosByTaskIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }
}
