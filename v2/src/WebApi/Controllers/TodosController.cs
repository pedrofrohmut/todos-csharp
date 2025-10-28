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

[Route("api/v2/todos")]
public class TodosController : ControllerBase
{
    [HttpPost("")]
    public async Task CreateTodo(CreateTodoBody body)
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
