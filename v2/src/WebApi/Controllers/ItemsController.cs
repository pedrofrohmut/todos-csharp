using Microsoft.AspNetCore.Mvc;

namespace Todos.WebApi.Controllers;

[Route("api/v2/items")]
public class ItemsController : ControllerBase
{
    [HttpPost("")]
    public async Task CreateItem()
    {
    }

    [HttpGet("todo/{todoId}")]
    public async Task FindAllItemsByTodoId(int todoId)
    {
    }

    [HttpGet("{itemId}")]
    public async Task FindItemById(int itemId)
    {
    }

    [HttpPatch("setdone/{itemId}")]
    public async Task SetItemDone(int itemId)
    {
    }

    [HttpPatch("setnotdone/{itemId}")]
    public async Task SetItemNotDone(int itemId)
    {
    }

    [HttpPut("{itemId}")]
    public async Task UpdateItem(int itemId)
    {
    }

    [HttpDelete("{itemId}")]
    public async Task DeleteItem(int itemId)
    {
    }

    [HttpDelete("done")]
    public async Task DeleteDoneItems()
    {
    }

    [HttpDelete("done/todo/{todoId}")]
    public async Task DeleteDoneItemsByTodoId(int todoId)
    {
    }
}
