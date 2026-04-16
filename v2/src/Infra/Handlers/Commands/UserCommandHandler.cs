using System.Data;
using Dapper;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Infra.Handlers.Commands;

public class UserCommandHandler : IUserCommandHandler
{
    private readonly IDbConnection writeConnection;
    private readonly IDbConnection readConnection;

    public UserCommandHandler(IDbConnection writeConnection, IDbConnection readConnection)
    {
        this.writeConnection = writeConnection;
        this.readConnection = readConnection;
    }

    public async Task CreateUser(CreateUserCommand command)
    {
        var insertSql = @"
            INSERT INTO users (name, email, password_hash)
            VALUES (@Name, @Email, @PasswordHash)
            RETURNING id";

        var userId = await this.writeConnection.ExecuteScalarAsync<int>(insertSql, new {
            command.Name,
            command.Email,
            command.PasswordHash
        });

        try {
            await this.readConnection.ExecuteAsync(insertSql, new {
                command.Name,
                command.Email,
                command.PasswordHash
            });
        } catch {
            var deleteSql = "DELETE FROM users WHERE id = @Id";
            await this.writeConnection.ExecuteAsync(deleteSql, new { Id = userId });
            throw;
        }
    }
}
