namespace Todos.Core.UseCases.Todos;

public interface ISetTodoNotDoneUseCase
{
    void Execute(string? todoId, string? authUserId);
    Task ExecuteAsync(string? todoId, string? authUserId);
}
