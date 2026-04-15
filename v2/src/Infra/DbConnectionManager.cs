using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Todos.Infra;

public static class DbConnectionManager
{
    public static IDbConnection GetWriteConnection(IConfiguration configuration)
    {
        // var username = configuration["write_db_username"];
        // var password = configuration["write_db_password"];
        // var connection_string = $"Host=localhost; Username={username};" +
        //                         $"Password={password}; Database=write_todos_csharp";
        var connection_string =
            "Host=localhost; Port=5432; Username=write_user; Password=write_password; Database=write_db";
        return new NpgsqlConnection(connection_string);
    }

    public static IDbConnection GetReadConnection(IConfiguration configuration)
    {
        // var username = configuration["read_db_username"];
        // var password = configuration["read_db_password"];
        // var connection_string = $"Host=localhost; Username={username};" +
        //                         $"Password={password}; Database=read_todos_csharp";
        var connection_string =
            "Host=localhost; Port=5433; Username=read_user; Password=read_password; Database=read_db";
        return new NpgsqlConnection(connection_string);
    }

    public static void OpenConnection(IDbConnection? connection)
    {
        if (connection == null) {
            throw new ArgumentNullException("Connection is null and cannot be opened");
        }
        try {
            connection.Open();
        } catch (Exception e) {
            throw new Exception("Error to open database connection. " + e.Message);
        }
    }

    public static void CloseConnection(IDbConnection? connection)
    {
        if (connection == null) {
            throw new ArgumentNullException("Connection is null and cannot be closed");
        }
        try {
            if (connection.State == ConnectionState.Open) {
                connection.Close();
            }
        } catch (Exception e) {
            throw new Exception("Error to close database connection. " + e.Message);
        }
    }
}
