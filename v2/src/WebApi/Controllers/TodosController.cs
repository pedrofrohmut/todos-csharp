using Microsoft.AspNetCore.Mvc;
using Todos.Core.UseCases.Todos;
using Todos.Infra;
using Todos.Core.Utils;
using Todos.Core.Errors;

namespace Todos.WebApi.Controllers;

public readonly struct CreateTodoBody
{
    public string Name { get; init; }
    public string Description { get; init; }
}

/*
   TODO:
   1. Add a way to get auth token from the request headers
   2. Pass the authToken in the useCaseInput
*/
[Route("api/v2/todos")]
public class TodosController : ControllerBase
{
    [HttpPost("")]
    public async Task CreateTodo([FromBody] CreateTodoBody body)
    {
        var useCase = UseCasesFactory.GetCreateTodoUseCase();
        var input = new CreateTodoInput {
            Name = body.Name,
            Description = body.Description,
        };

        Result<CreateTodoOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

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
    }

    [HttpDelete("{todoId}")]
    public async Task DeleteTodo(int todoId)
    {
        var useCase = UseCasesFactory.GetDeleteTodoUseCase();
        var input = new DeleteTodoInput { Id = todoId };

        Result<DeleteTodoOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

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

        if (result.Error is InvalidTokenError) {
            HttpContext.Response.StatusCode = 401;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
    }

    [HttpGet("{todoId}")]
    public async Task FindTodoById(int todoId)
    {
        var useCase = UseCasesFactory.GetFindTodoByIdUseCase();
        var input = new FindTodoByIdInput { Id = todoId };

        Result<FindTodoByIdOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

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

        if (result.Error is InvalidTokenError) {
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
    }

    [HttpGet("")]
    public async Task FindAllTodos()
    {
        var useCase = UseCasesFactory.GetFindAllTodosUseCase();
        var input = new FindAllTodosInput { };

        Result<FindAllTodosOutput> result;
        try {
            result = await useCase.Execute(input);
        } catch (Exception e) {
            await ControllerUtils.WriteExceptionResponse(HttpContext, e);
            return;
        }

        if (result.IsSuccess) {
            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(result.Payload);
            return;
        }

        if (result.Error is InvalidTokenError) {
            HttpContext.Response.StatusCode = 401;
            await HttpContext.Response.WriteAsync(result.Error.Message);
            return;
        }

        await ControllerUtils.WriteErrorNotMappedResponse(HttpContext);
    }

    [HttpPut("{todoId}")]
    public async Task UpdateTodo(int todoId)
    {
    }
}
