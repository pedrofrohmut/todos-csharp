namespace Todos.Core.UseCases.Todos;

public interface IDeleteDoneTodosByTaskIdUseCase
{
    void Execute(string? taskId, string? authUserId);
    Task ExecuteAsync(string? taskId, string? authUserId);
}
