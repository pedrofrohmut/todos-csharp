using System.Data;
using Dapper;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;
using Todos.Core.Db;

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
        var insertSql = String.Join(" ", new string[] {
            "INSERT INTO todos (name, description, user_id)",
            "VALUES (@Name, @Description, @UserId)",
            "RETURNING id",
        });

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

    public async Task DeleteTodo(TodoDeleteCommand command)
    {
        var findByIdSql = String.Join(" ", new string[] {
            "SELECT id, name, description, is_done as IsDone, user_id as UserId, created_at as CreatedAt, updated_at as UpdatedAt",
            "FROM todos WHERE id = @TodoId",
        });
        // Backup row before everything. If the backup fails you wont even start to change data
        TodoDb backupTodo = await this.readConnection.QueryFirstAsync<TodoDb>(findByIdSql, new { TodoId = command.Id });

        // Delete from Read Db first
        var deleteSql = "DELETE FROM todos WHERE id = @TodoId";
        await this.readConnection.ExecuteAsync(deleteSql, new { TodoId = command.Id });

        // Delete from Write Db or restore the todo in readDb in case of error
        try {
            await this.writeConnection.ExecuteAsync(deleteSql, new { TodoId = command.Id });
        } catch {
            var insertSql = String.Join(" ", new string[] {
                "INSERT INTO todos (id, name, description, is_done, user_id, created_at, updated_at)",
                "VALUE (@Id, @Name, @Description, @IsDone, @UserId, @CreatedAt, @UpdatedAt)"
            });
            await this.readConnection.ExecuteAsync(insertSql, new {
                Id = backupTodo.Id,
                Name = backupTodo.Name,
                Description = backupTodo.Description,
                IsDone = backupTodo.IsDone,
                UserId = backupTodo.UserId,
                CreatedAt = backupTodo.CreatedAt,
                UpdatedAt = backupTodo.UpdatedAt,
            });
        }
    }

    public Task UpdateTodo(TodoUpdateCommand command)
    {
        throw new NotImplementedException();
    }
}
