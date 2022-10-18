namespace Todos.Core.UseCases.Todos;

public interface IDeleteTodoUseCase
{
    void Execute(string? todoId, string? authUserId);
}
