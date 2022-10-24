using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Todos;

public interface ICreateTodoUseCase
{
    void Execute(CreateTodoDto? newTodo, string? authUserId);
}
