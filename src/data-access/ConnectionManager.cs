using System.Data;
using Todos.Core.DataAccess;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Todos.DataAccess;

public class ConnectionManager : IConnectionManager
{
    public IDbConnection GetConnection(IConfiguration configuration)
    {
        var username = configuration["username"];
        var password = configuration["password"];
        var str = $"Host=localhost; Username={username}; Password={password}; Database=goals_db";
        return new NpgsqlConnection(str);
    }

    public void OpenConnection(IDbConnection connection)
    {
        try {
            connection.Open();
        } catch (Exception e) {
            throw new Exception("Error to open database connection. " + e.Message);
        }
    }

    public void CloseConnection(IDbConnection connection)
    {
        try {
            if (connection.State == ConnectionState.Open) {
                connection.Close();
            }
        } catch (Exception e) {
            throw new Exception("Error to close database connection. " + e.Message);
        }
    }
}
