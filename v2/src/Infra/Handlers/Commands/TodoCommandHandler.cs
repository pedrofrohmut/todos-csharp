using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class TodoCommandHandler : ITodoCommandHandler
{
    public Task CreateTodo(TodoCreateCommand command)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTodo(TodoDeleteCommand command)
    {
        throw new NotImplementedException();
    }
}
