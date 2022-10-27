using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface ITaskDataAccess
{
    // Sync
    void Create(CreateTaskDto newTask, string userId);
    void Delete(string taskId);
    TaskDbDto? FindById(string taskId);
    List<TaskDbDto> FindByUserId(string userId);
    void Update(string taskId, UpdateTaskDto updatedTask);
    // Async
    Task CreateAsync(CreateTaskDto newTask, string userId);
    Task DeleteAsync(string taskId);
    Task<TaskDbDto?> FindByIdAsync(string taskId);
    Task<List<TaskDbDto>> FindByUserIdAsync(string userId);
    Task UpdateAsync(string taskId, UpdateTaskDto updatedTask);
}
