using Microsoft.AspNetCore.Mvc;
using Todos.Infra;
using Todos.Core.UseCases.Items;
using Todos.Core.Errors;

namespace Todos.WebApi.Controllers;

public record CreateItemBody(
    string Name,
    string Description,
    int TodoId
);

[Route("api/v2/items")]
public class ItemsController : ControllerBase
{
    [HttpPost("")]
    public async Task CreateItem([FromBody] CreateItemBody body)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection();
        var readConnection = DbConnectionManager.GetReadConnection();

        try {
            var useCase = UseCasesFactory.GetCreateItemUseCase(writeConnection, readConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new CreateItemInput {
                Name = body.Name,
                Description = body.Description,
                TodoId = body.TodoId,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                await ControllerUtils.SetResponse(HttpContext, 201, "Create Item: Item created successfully.");
                return;
            }

            if (result.Error is InvalidItemError) {
                await ControllerUtils.SetResponse(HttpContext, 400, result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError) {
                await ControllerUtils.SetResponse(HttpContext, 401, result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(CreateItem), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
            DbConnectionManager.CloseConnection(readConnection);
        }
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
