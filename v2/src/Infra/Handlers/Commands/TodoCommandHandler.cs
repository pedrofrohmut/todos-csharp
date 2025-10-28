using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class TodoCommandHandler : ITodoCommandHandler
{
    public Task CreateTodo(CreateTodoCommand command)
    {
        throw new NotImplementedException();
    }
}
