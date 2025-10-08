using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface IFindTasksByUserIdUseCase
{
    List<TaskDto> Execute(string? authUserId);
    Task<List<TaskDto>> ExecuteAsync(string? authUserId);
}
