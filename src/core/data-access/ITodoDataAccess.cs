using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface ITodoDataAccess
{
    void Create(CreateTodoDto newTodo, string userId);
}