namespace Todos.Core.UseCases.Todos;

public interface ISetTodoDoneUseCase
{
    void Execute(string? todoId, string? authUserId);
}
