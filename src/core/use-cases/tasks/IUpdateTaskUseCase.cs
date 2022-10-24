using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface IUpdateTaskUseCase
{
    void Execute(string? taskId, UpdateTaskDto? updatedTask, string? authUserId);
}
