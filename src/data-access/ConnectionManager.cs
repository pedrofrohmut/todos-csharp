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
        var connection_string = $"Host=localhost; Username={username};" + 
                                $"Password={password}; Database=todos_csharp";
        return new NpgsqlConnection(connection_string);
    }

    public void OpenConnection(IDbConnection? connection)
    {
        if (connection == null) {
            throw new ArgumentNullException("Connection is null and cannot be opened");
        }
        try {
            connection.Open();
            Console.WriteLine("Connection Opened");
        } catch (Exception e) {
            throw new Exception("Error to open database connection. " + e.Message);
        }
    }

    public void CloseConnection(IDbConnection? connection)
    {
        if (connection == null) {
            throw new ArgumentNullException("Connection is null and cannot be closed");
        }
        try {
            if (connection.State == ConnectionState.Open) {
                connection.Close();
                Console.WriteLine("Connection Closed");
            }
        } catch (Exception e) {
            throw new Exception("Error to close database connection. " + e.Message);
        }
    }
}
