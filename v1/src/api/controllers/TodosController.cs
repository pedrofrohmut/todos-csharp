using System.Data;
using Microsoft.AspNetCore.Mvc;
using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.UseCases.Todos;
using Todos.Core.WebIO;
using Todos.DataAccess;

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

    [HttpPost("")]
    public ActionResult Create([FromBody] CreateTodoDto newTodo)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var createTodoUseCase = new CreateTodoUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Body = newTodo, AuthUserId = authUserId };
        var response = TodosWebIO.Create(createTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPost("async")]
    public async Task<ActionResult> CreateAsync([FromBody] CreateTodoDto newTodo)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var createTodoUseCase = new CreateTodoUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Body = newTodo, AuthUserId = authUserId };
        var response = await TodosWebIO.CreateAsync(createTodoUseCase, webRequest);
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
        var response = TodosWebIO.FindByTaskId(findTodosByTaskIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("task/{id}/async")]
    public async Task<ActionResult> FindAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodosByTaskIdUseCase =
            new FindTodosByTaskIdUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.FindByTaskIdAsync(findTodosByTaskIdUseCase, webRequest);
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
        var response = TodosWebIO.FindById(findTodoByIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("{id}/async")]
    public async Task<ActionResult> FindByIdAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var findTodoByIdUseCase = new FindTodoByIdUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.FindByIdAsync(findTodoByIdUseCase, webRequest);
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
        var response = TodosWebIO.SetDone(setTodoDoneUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPatch("setdone/{id}/async")]
    public async Task<ActionResult> SetDoneAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoDoneUseCase = new SetTodoDoneUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.SetDoneAsync(setTodoDoneUseCase, webRequest);
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
        var response = TodosWebIO.SetNotDone(setTodoNotDoneUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPatch("setnotdone/{id}/async")]
    public async Task<ActionResult> SetNotDoneAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var setTodoNotDoneUseCase = new SetTodoNotDoneUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.SetNotDoneAsync(setTodoNotDoneUseCase, webRequest);
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
        var response = TodosWebIO.Update(updateTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPut("{id}/async")]
    public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdateTodoDto updatedTodo)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var updateTodoUseCase = new UpdateTodoUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, Body = updatedTodo, AuthUserId = authUserId };
        var response = await TodosWebIO.UpdateAsync(updateTodoUseCase, webRequest);
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
        var response = TodosWebIO.Delete(deleteTodoUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("{id}/async")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteTodoUseCase = new DeleteTodoUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.DeleteAsync(deleteTodoUseCase, webRequest);
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
        var response = TodosWebIO.DeleteDone(deleteDoneTodosUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("done/async")]
    public async Task<ActionResult> DeleteDoneAsync()
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteDoneTodosUseCase = new DeleteDoneTodosUseCase(userDataAccess, todoDataAccess);
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var response = await TodosWebIO.DeleteDoneAsync(deleteDoneTodosUseCase, webRequest);
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
        var response = TodosWebIO.DeleteDoneByTaskId(deleteDoneTodosByTaskIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("done/task/{id}/async")]
    public async Task<ActionResult> DeleteDoneByTaskIdAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var todoDataAccess = new TodoDataAccess(connection);
        var deleteDoneTodosByTaskIdUseCase =
            new DeleteDoneTodosByTaskIdUseCase(userDataAccess, taskDataAccess, todoDataAccess);
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TodosWebIO.DeleteDoneByTaskIdAsync(deleteDoneTodosByTaskIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }
}
