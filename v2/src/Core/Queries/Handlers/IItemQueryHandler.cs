using Todos.Core.Db;

namespace Todos.Core.Queries.Handlers;

public interface IItemQueryHandler
{
    Task<IEnumerable<ItemDb>> FindAllItemByTodoId(ItemFindAllByTodoIdQuery query);
}
