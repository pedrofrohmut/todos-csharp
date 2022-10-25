using System.Data;
using Dapper;
using Todos.Core.DataAccess;
using Todos.Core.Dtos;

namespace Todos.DataAccess;

public class UserDataAccess : IUserDataAccess
{
    private readonly IDbConnection connection;

    public UserDataAccess(IDbConnection connection)
    {
        this.connection = connection;
    }

    public void Create(CreateUserDto newUser, string passwordHash)
    {
        var sql = @"INSERT INTO app.users (name, email, password_hash)
                    VALUES (@name, @email, @passwordHash)";
        this.connection.Query(sql, new {
            @name = newUser.Name,
            @email = newUser.Email,
            @passwordHash = passwordHash
        });
    }

    public Task CreateAsync(CreateUserDto newUser, string passwordHash)
    {
        var sql = @"INSERT INTO app.users (name, email, password_hash)
                    VALUES (@name, @email, @passwordHash)";
        return this.connection.QueryAsync(sql, new {
            @name = newUser.Name,
            @email = newUser.Email,
            @passwordHash = passwordHash
        });
    }

    public UserDbDto? FindByEmail(string email)
    {
        var sql = @"SELECT id, name, email, password_hash as passwordHash
                    FROM app.users
                    WHERE email = @email";
        var user = this.connection.Query<UserDbDto>(sql, new { email }).SingleOrDefault();
        return user;
    }

    public async Task<UserDbDto?> FindByEmailAsync(string email)
    {
        var sql = @"SELECT id, name, email, password_hash as passwordHash
                    FROM app.users
                    WHERE email = @email";
        var result = await this.connection.QueryAsync<UserDbDto>(sql, new { email });
        var user = result.SingleOrDefault();
        return user;
    }

    public UserDbDto? FindById(string userId)
    {
        var sql = @"SELECT id, name, email, password_hash as passwordHash
                    FROM app.users
                    WHERE id = @userId";
        var user = this.connection.Query<UserDbDto>(sql, new { @userId = Guid.Parse(userId) })
                                  .SingleOrDefault();
        return user;
    }

    public async Task<UserDbDto?> FindByIdAsync(string userId)
    {
        var sql = @"SELECT id, name, email, password_hash as passwordHash
                    FROM app.users
                    WHERE id = @userId";
        var row = await this.connection.QueryAsync<UserDbDto>(sql, new { @userId = Guid.Parse(userId) });
        var user = row.SingleOrDefault();
        return user;
    }
}
