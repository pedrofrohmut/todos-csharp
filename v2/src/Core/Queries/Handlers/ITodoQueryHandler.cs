using Todos.Core.Db;

namespace Todos.Core.Queries.Handlers;

public interface ITodoQueryHandler
{
    Task<TodoDb> FindTodoById(TodoFindByIdQuery query);
}
