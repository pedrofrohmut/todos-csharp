using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Todos;

public interface IFindTodosByTaskIdUseCase
{
    List<TodoDto> Execute(string? taskId, string? authUserId);
    Task<List<TodoDto>> ExecuteAsync(string? taskId, string? authUserId);
}
