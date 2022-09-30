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

    public void CreateUser(CreateUserDto newUser, string passwordHash)
    {
        var sql = @"INSERT INTO app.users (name, email, password_hash) 
                    VALUES (@name, @email, @passwordHash)";
        this.connection.Query(sql, new {
            @name = newUser.Name,
            @email = newUser.Email,
            @passwordHash = passwordHash
        });
    }

    public UserDbDto? FindUserByEmail(string email)
    {
        var sql = @"SELECT * FROM app.users WHERE email = @email";
        var row = this.connection.Query(sql, new { email }).SingleOrDefault();
        if (row == null) return null;
        var user = new UserDbDto() {
            Id = row.id.ToString(),
            Name = row.name,
            Email = row.email,
            PasswordHash = row.password_hash
        };
        return user;
    }

    public UserDbDto? FindUserById(string userId)
    {
        var sql = @"SELECT * FROM app.users WHERE id = @id";
        var row = this.connection.Query(sql, new { id = Guid.Parse(userId) })
                                 .SingleOrDefault();
        if (row == null) return null;
        var user = new UserDbDto() {
            Id = row.id.ToString(),
            Name = row.name,
            Email = row.email,
            PasswordHash = row.password_hash
        };
        return user;
    }
}
