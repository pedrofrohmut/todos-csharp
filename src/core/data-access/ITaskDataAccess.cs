using Todos.Core.Dtos;

namespace Todos.Core.DataAccess;

public interface ITaskDataAccess
{
    void Create(CreateTaskDto newTask, string userId);
}
