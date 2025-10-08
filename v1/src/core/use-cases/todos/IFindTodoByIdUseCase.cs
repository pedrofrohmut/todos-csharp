using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Todos;

public interface IFindTodoByIdUseCase
{
    TodoDto Execute(string? todoId, string? authUserId);
    Task<TodoDto> ExecuteAsync(string? todoId, string? authUserId);
}
