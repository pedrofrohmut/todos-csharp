using System.Data;
using Dapper;
using Todos.Core.Db;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;

namespace Todos.Infra.Handlers.Queries;

public class TodoQueryHandler : ITodoQueryHandler
{
    private readonly IDbConnection connection;

    public TodoQueryHandler(IDbConnection connection)
    {
        this.connection = connection;
    }

    public Task<TodoDb?> FindTodoById(TodoFindByIdQuery query)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TodoDb>> FindAllTodos(TodoFindAllQuery query)
    {
        var sql = String.Join(" ", new string[] {
            "SELECT id, name, description, user_id as UserId, is_done as IsDone",
            "FROM todos",
            "WHERE user_id = @UserId",
        });
        var todos = await this.connection.QueryAsync<TodoDb>(sql, new { UserId = query.UserId });
        return todos.ToList();
    }
}
