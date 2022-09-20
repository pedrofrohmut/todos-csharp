using System.Data;
using Todos.Core.DataAccess;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Todos.DataAccess;

public class ConnectionManager : IConnectionManager
{
    private readonly IConfiguration configuration;

    public ConnectionManager(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
   
    public IDbConnection GetConnection()
    {
        var username = this.configuration["username"];
        var password = this.configuration["password"];
        var connection_string = $"Host=localhost; Username={username};" + 
                                $"Password={password}; Database=todos_csharp";
        return new NpgsqlConnection(connection_string);
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
