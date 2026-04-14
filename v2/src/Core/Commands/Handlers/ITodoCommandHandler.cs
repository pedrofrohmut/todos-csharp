namespace Todos.Core.Commands.Handlers;

public interface ITodoCommandHandler
{
    Task CreateTodo(TodoCreateCommand command);
    Task UpdateTodo(TodoUpdateCommand command);
    Task DeleteTodo(TodoDeleteCommand command);
}
