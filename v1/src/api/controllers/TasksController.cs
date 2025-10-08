using Microsoft.AspNetCore.Mvc;

using Todos.Core.WebIO;
using Todos.Core.Dtos;
using Todos.Core.DataAccess;
using Todos.Core.UseCases.Tasks;
using Todos.DataAccess;
using System.Data;

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

    [HttpPost("")]
    public ActionResult Create([FromBody] CreateTaskDto newTask)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var createTaskUseCase = new CreateTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Body = newTask, AuthUserId = authUserId };
        var response = TasksWebIO.Create(createTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPost("async")]
    public async Task<ActionResult> CreateAsync([FromBody] CreateTaskDto newTask)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var createTaskUseCase = new CreateTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Body = newTask, AuthUserId = authUserId };
        var response = await TasksWebIO.CreateAsync(createTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var deleteTaskUseCase = new DeleteTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = TasksWebIO.Delete(deleteTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpDelete("{id}/async")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var connection = (IDbConnection) HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var deleteTaskUseCase = new DeleteTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TasksWebIO.DeleteAsync(deleteTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("{id}")]
    public ActionResult FindById(string id)
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var findTaskByIdUseCase = new FindTaskByIdUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = TasksWebIO.FindById(findTaskByIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("{id}/async")]
    public async Task<ActionResult> FindByIdAsync(string id)
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var findTaskByIdUseCase = new FindTaskByIdUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id, AuthUserId = authUserId };
        var response = await TasksWebIO.FindByIdAsync(findTaskByIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("")]
    public ActionResult FindByUserId()
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var findTasksByUserIdUseCase = new FindTasksByUserIdUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var response = TasksWebIO.FindByUserId(findTasksByUserIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpGet("async")]
    public async Task<ActionResult> FindByUserIdAsync()
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var findTasksByUserIdUseCase = new FindTasksByUserIdUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { AuthUserId = authUserId };
        var response = await TasksWebIO.FindByUserIdAsync(findTasksByUserIdUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPut("{id}")]
    public ActionResult UpdateTask(string id, [FromBody] UpdateTaskDto updatedTask)
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var updateTaskUseCase = new UpdateTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id,
                                               Body = updatedTask,
                                               AuthUserId = authUserId };
        var response = TasksWebIO.Update(updateTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }

    [HttpPut("{id}/async")]
    public async Task<ActionResult> UpdateTaskAsync(string id, [FromBody] UpdateTaskDto updatedTask)
    {
        var connection = (IDbConnection)HttpContext.Items["connection"]!;
        var authUserId = Convert.ToString(HttpContext.Items["authUserId"]);
        var userDataAccess = new UserDataAccess(connection);
        var taskDataAccess = new TaskDataAccess(connection);
        var updateTaskUseCase = new UpdateTaskUseCase(userDataAccess, taskDataAccess);
        var webRequest = new WebRequestDto() { Param = id,
                                               Body = updatedTask,
                                               AuthUserId = authUserId };
        var response = await TasksWebIO.UpdateAsync(updateTaskUseCase, webRequest);
        return new ObjectResult(response.Value) { StatusCode = response.Status };
    }
}
