using System.Data;
using Dapper;
using Todos.Core.Db;
using Todos.Core.Queries;
using Todos.Core.Queries.Handlers;

namespace Todos.Infra.Handlers.Queries;

public class UserQueryHandler : IUserQueryHandler
{
    private readonly IDbConnection connection;

    public UserQueryHandler(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<UserDb?> FindUserByEmail(UserFindByEmailQuery query)
    {
        var sql = String.Join(" ", new string[] {
            "SELECT id, name, email, password_hash AS PasswordHash",
            "FROM users",
            "WHERE email = @Email",
        });
        var user = await this.connection.QueryFirstOrDefaultAsync<UserDb>(sql, new { Email = query.Email });
        if (user.Id == 0) {
            return null;
        }
        return user;
    }

    public async Task<UserDb?> FindUserById(UserFindByIdQuery query)
    {
        var sql = String.Join(" ", new string[] {
            "SELECT id, name, email, password_hash AS PasswordHash",
            "FROM users",
            "WHERE id = @Id",
        });
        var user = await this.connection.QueryFirstOrDefaultAsync<UserDb>(sql, new { Id = query.Id });
        if (user.Id == 0) {
            return null;
        }
        return user;
    }
}
