using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface ITodoDataAccess
{
    // Sync
    void Create(CreateTodoDto newTodo, string userId);
    List<TodoDbDto>? FindByTaskId(string taskId);
    TodoDbDto? FindById(string todoId);
    void SetDone(string todoId);
    void SetNotDone(string todoId);
    void Update(string todoId, UpdateTodoDto updatedTodo);
    void Delete(string todoId);
    void DeleteDone(string userId);
    void DeleteDoneByTaskId(string taskId);
    // Async
    Task CreateAsync(CreateTodoDto newTodo, string userId);
    Task<List<TodoDbDto>?> FindByTaskIdAsync(string taskId);
    Task<TodoDbDto?> FindByIdAsync(string todoId);
    Task SetDoneAsync(string todoId);
    Task SetNotDoneAsync(string todoId);
    Task UpdateAsync(string todoId, UpdateTodoDto updatedTodo);
    Task DeleteAsync(string todoId);
    Task DeleteDoneAsync(string userId);
    Task DeleteDoneByTaskIdAsync(string taskId);
}
