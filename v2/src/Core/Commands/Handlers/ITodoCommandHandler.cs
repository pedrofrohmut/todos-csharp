namespace Todos.Core.Commands.Handlers;

public interface ITodoCommandHandler
{
    Task CreateTodo(TodoCreateCommand command);
    Task DeleteTodo(TodoDeleteCommand command);
}
