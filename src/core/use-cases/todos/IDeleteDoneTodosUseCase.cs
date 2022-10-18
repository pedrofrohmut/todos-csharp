namespace Todos.Core.UseCases.Todos;

public interface IDeleteDoneTodosUseCase
{
    void Execute(string? authUserId);
}
