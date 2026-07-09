using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using DotNetEnv;

namespace Todos.Infra;

public static class DbConnectionManager
{
    public static IDbConnection GetWriteConnection()
    {
        var host = Environment.GetEnvironmentVariable("WRITE_DB_HOST");
        var port = Environment.GetEnvironmentVariable("WRITE_DB_PORT");
        var username = Environment.GetEnvironmentVariable("WRITE_DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("WRITE_DB_PASSWORD");
        var dbName = Environment.GetEnvironmentVariable("WRITE_DB_DBNAME");

        var connectionString = $"Host={host}; Port={port}; Username={username};Password={password}; Database={dbName};";

        return new NpgsqlConnection(connectionString);
    }

    public static IDbConnection GetReadConnection()
    {
        var host = Environment.GetEnvironmentVariable("READ_DB_HOST");
        var port = Environment.GetEnvironmentVariable("READ_DB_PORT");
        var username = Environment.GetEnvironmentVariable("READ_DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("READ_DB_PASSWORD");
        var dbName = Environment.GetEnvironmentVariable("READ_DB_DBNAME");

        var connectionString = $"Host={host}; Port={port}; Username={username};Password={password}; Database={dbName};";

        return new NpgsqlConnection(connectionString);
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
