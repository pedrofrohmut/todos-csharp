using System.Data;
using Dapper;
using Todos.Core.DataAccess;
using Todos.Core.Dtos;

namespace Todos.DataAccess;

public class TaskDataAccess : ITaskDataAccess
{
    private readonly IDbConnection connection;

    public TaskDataAccess(IDbConnection connection)
    {
        this.connection = connection;
    }

    public void Create(CreateTaskDto newTask, string userId)
    {
        var sql = @"INSERT INTO app.tasks (name, description, user_id)
                    VALUES (@name, @description, @userId)";
        this.connection.Query(sql, new {
            @name = newTask.Name,
            @description = newTask.Description,
            @userId = Guid.Parse(userId)
        });
    }
}
