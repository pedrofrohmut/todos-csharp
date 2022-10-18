namespace Todos.Core.UseCases.Todos;

public interface IDeleteCompletedTodosUseCase
{
    void Execute(string? authUserId);
}
