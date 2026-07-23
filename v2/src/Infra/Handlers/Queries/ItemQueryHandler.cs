using System.Data;
using Dapper;
using Todos.Core.Db;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;

namespace Todos.Infra.Handlers.Queries;

public class ItemQueryHandler : IItemQueryHandler
{
    private readonly IDbConnection connection;

    public ItemQueryHandler(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<ItemDb>> FindAllItemByTodoId(ItemFindAllByTodoIdQuery query)
    {
        var sql = String.Join(" ", new string[] {
            "SELECT id, name, description, is_done as IsDone, user_id as UserId",
            "FROM todo_items",
            "WHERE todo_id = @TodoId",
        });
        return await this.connection.QueryAsync<ItemDb>(sql, new { TodoId = query.TodoId });
    }
}
