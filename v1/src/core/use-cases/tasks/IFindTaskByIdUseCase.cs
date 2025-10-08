using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface IFindTaskByIdUseCase
{
    TaskDto Execute(string? taskId, string? authUserId);
    Task<TaskDto> ExecuteAsync(string? taskId, string? authUserId);
}
