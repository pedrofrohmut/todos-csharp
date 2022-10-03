namespace Todos.Core.UseCases.Tasks;

public interface IDeleteTaskUseCase
{
    void Execute(string taskId, string userId);
}
