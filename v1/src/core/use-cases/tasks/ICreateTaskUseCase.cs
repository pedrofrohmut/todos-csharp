using Todos.Core.Dtos;

namespace Todos.Core.UseCases.Tasks;

public interface ICreateTaskUseCase
{
    void Execute(CreateTaskDto? newTask, string? authUserId);
    Task ExecuteAsync(CreateTaskDto? newTask, string? authUserId);
}
