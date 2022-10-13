using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface ITodoDataAccess
{
    void Create(CreateTodoDto newTodo, string userId);
    List<TodoDbDto> FindByTaskId(string taskId);
    TodoDbDto? FindById(string todoId);
    void SetDone(string todoId);
    void SetNotDone(string todoId);
}
