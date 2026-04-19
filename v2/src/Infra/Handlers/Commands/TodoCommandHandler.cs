using System.Data;
using Dapper;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class TodoCommandHandler : ITodoCommandHandler
{
    private readonly IDbConnection writeConnection;
    private readonly IDbConnection readConnection;

    public TodoCommandHandler(IDbConnection writeConnection, IDbConnection readConnection)
    {
        this.writeConnection = writeConnection;
        this.readConnection = readConnection;
    }

    public async Task CreateTodo(TodoCreateCommand command)
    {
        var insertSql = @"
            INSERT INTO todos (name, description, user_id)
            VALUES (@Name, @Description, @UserId)
            RETURNING id";

        var todoId = await this.writeConnection.ExecuteScalarAsync<int>(insertSql, new {
            command.Name,
            command.Description,
            command.UserId
        });

        try {
            await this.readConnection.ExecuteAsync(insertSql, new {
                command.Name,
                command.Description,
                command.UserId
            });
        } catch {
            var deleteSql = "DELETE FROM todos WHERE id = @Id";
            await this.writeConnection.ExecuteAsync(deleteSql, new { Id = todoId });
            throw;
        }
    }

    public Task DeleteTodo(TodoDeleteCommand command)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTodo(TodoUpdateCommand command)
    {
        throw new NotImplementedException();
    }
}
