using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Todos;

public interface IUpdateTodoUseCase
{
    void Execute(string? todoId, UpdateTodoDto? updatedTodo, string? authUserId);
}
