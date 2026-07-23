using Microsoft.AspNetCore.Mvc;
using Todos.Infra;
using Todos.Core.UseCases.Items;
using Todos.Core.Errors;
using System.Text.Json;
using Todos.Core.Utils;

namespace Todos.WebApi.Controllers;

public record CreateItemBody(
    string Name,
    string Description,
    int TodoId
);

[Route("api/v2/items")]
public class ItemsController : ControllerBase
{
    // TODO: Check if this endpoint is working
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
                await ControllerUtils.SetResponseText(HttpContext, 201, "Create Item: Item created successfully.");
                return;
            }

            if (result.Error is InvalidItemError) {
                await ControllerUtils.SetResponseText(HttpContext, 400, result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError) {
                await ControllerUtils.SetResponseText(HttpContext, 401, result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext, result.Error);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(CreateItem), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
            DbConnectionManager.CloseConnection(readConnection);
        }
    }

    [HttpGet("todo/{todoId}")]
    public async Task FindAllItemsByTodoId([FromRoute] int todoId)
    {
        var readConnection = DbConnectionManager.GetReadConnection();

        try {
            var useCase = UseCasesFactory.GetFindAllItemsByTodoIdUseCase(readConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new FindAllItemsByTodoIdInput {
                TodoId = todoId,
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                await ControllerUtils.SetResponseJson(HttpContext, 200, result.Payload);
                return;
            }

            if (result.Error is InvalidItemError or TodoNotFoundError) {
                await ControllerUtils.SetResponseText(HttpContext, 400, result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError or TodoOwnershipError) {
                await ControllerUtils.SetResponseText(HttpContext, 401, result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext, result.Error);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(FindAllItemsByTodoId), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(readConnection);
        }
    }

    [HttpGet("{itemId}")]
    public async Task FindItemById([FromRoute] int itemId)
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
