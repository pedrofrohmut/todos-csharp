using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface IFindTasksByUserIdUseCase
{
    IEnumerable<TaskDto> Execute(string authUserId);
}
