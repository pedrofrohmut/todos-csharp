namespace Todos.Core.Commands.Handlers;

public interface ITodoCommandHandler
{
    Task CreateTodo(CreateTodoCommand command);
}
