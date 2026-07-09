namespace Todos.Core.Commands.Handlers;

public interface IItemCommandHandler
{
    Task CreateItem(ItemCreateCommand command);
    Task UpdateItem(ItemUpdateCommand command);
    Task DeleteItem(ItemDeleteCommand command);
}
