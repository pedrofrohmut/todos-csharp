using Microsoft.AspNetCore.Mvc;

namespace Todos.WebApi.Controllers;

[Route("api/v2/todos")]
public class TodosController : ControllerBase
{
    [HttpPost("")]
    public async Task CreateTodo()
    {
    }

    [HttpDelete("{todoId}")]
    public async Task DeleteTodo(int todoId)
    {
    }

    [HttpGet("{todoId}")]
    public async Task FindTodoById(int todoId)
    {
    }

    [HttpGet("")]
    public async Task FindAllTodos()
    {
    }

    [HttpPut("{todoId}")]
    public async Task UpdateTodo(int todoId)
    {
    }
}
