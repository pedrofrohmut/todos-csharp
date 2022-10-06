using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface IUpdateTaskUseCase
{
    void Execute(UpdateTaskDto updatedTask, string authUserId);
}
