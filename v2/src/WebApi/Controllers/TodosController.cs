using Microsoft.AspNetCore.Mvc;
using Todos.Core.UseCases.Todos;
using Todos.Infra;
using Todos.Core.Errors;

namespace Todos.WebApi.Controllers;

// public readonly struct CreateTodoBody
// {
//     public string Name { get; init; }
//     public string Description { get; init; }
// }

public record CreateTodoBody(
    string Name,
    string Description
);

public readonly struct UpdateTodoBody
{
    public string Name { get; init; }
    public string Description { get; init; }
}

[Route("api/v2/todos")]
public class TodosController : ControllerBase
{
    private readonly IConfiguration configuration;

    public TodosController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpPost("")]
    public async Task CreateTodo([FromBody] CreateTodoBody body)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetCreateTodoUseCase(writeConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new CreateTodoInput {
                Name = body.Name,
                Description = body.Description,
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 201;
                await HttpContext.Response.WriteAsync("Create Todo: Todo created successfully.");
                return;
            }

            if (result.Error is InvalidTodoError) {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError) {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(CreateTodo), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
        }

    }

    [HttpDelete("{todoId}")]
    public async Task DeleteTodo(int todoId)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetDeleteTodoUseCase(writeConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new DeleteTodoInput {
                Id = todoId,
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 204;
                await HttpContext.Response.WriteAsync("Delete Todo: Todo deleted successfully.");
                return;
            }

            if (result.Error is InvalidTodoError) {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError or TodoOwnershipError) {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(DeleteTodo), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
        }
    }

    [HttpGet("{todoId}")]
    public async Task FindTodoById(int todoId)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetFindTodoByIdUseCase(writeConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new FindTodoByIdInput {
                Id = todoId,
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 200;
                await HttpContext.Response.WriteAsJsonAsync(result.Payload);
                return;
            }

            if (result.Error is InvalidTodoError) {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError or TodoOwnershipError) {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is TodoNotFoundError) {
                HttpContext.Response.StatusCode = 404;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(FindTodoById), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
        }
    }

    [HttpGet("")]
    public async Task FindAllTodos()
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetFindAllTodosUseCase(writeConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new FindAllTodosInput {
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 200;
                await HttpContext.Response.WriteAsJsonAsync(result.Payload);
                return;
            }

            if (result.Error is InvalidTokenError or TodoOwnershipError) {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(FindAllTodos), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
        }
    }

    [HttpPut("{todoId}")]
    public async Task UpdateTodo(int todoId, [FromBody] UpdateTodoBody body)
    {
        var writeConnection = DbConnectionManager.GetWriteConnection(this.configuration);

        try {
            var useCase = UseCasesFactory.GetUpdateTodoUseCase(writeConnection);
            var authToken = ControllerUtils.GetAuthToken(HttpContext.Request);
            var input = new UpdateTodoInput {
                Id = todoId,
                Name = body.Name,
                Description = body.Description,
                AuthToken = authToken,
            };

            var result = await useCase.Execute(input);

            if (result.IsSuccess) {
                HttpContext.Response.StatusCode = 204;
                await HttpContext.Response.WriteAsync("");
                return;
            }

            if (result.Error is InvalidTodoError) {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is InvalidTokenError or TodoOwnershipError) {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            if (result.Error is TodoNotFoundError) {
                HttpContext.Response.StatusCode = 404;
                await HttpContext.Response.WriteAsync(result.Error.Message);
                return;
            }

            await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(nameof(UpdateTodo), HttpContext, e);
            return;
        } finally {
            DbConnectionManager.CloseConnection(writeConnection);
        }
    }
}
