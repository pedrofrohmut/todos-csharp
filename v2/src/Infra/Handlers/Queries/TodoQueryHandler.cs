using Todos.Core.Db;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;

namespace Todos.Infra.Handlers.Queries;

public class TodoQueryHandler : ITodoQueryHandler
{
    public Task<TodoDb?> FindTodoById(TodoFindByIdQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TodoDb>> FindAllTodos(TodoFindAllQuery query)
    {
        throw new NotImplementedException();
    }
}
