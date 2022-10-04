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

    public void Delete(string taskId)
    {
        var sql = "DELETE FROM app.tasks WHERE id = @taskId";
        this.connection.Query(sql, new { @taskId = Guid.Parse(taskId) });
    }

    public TaskDbDto? FindById(string taskId)
    {
        var sql = "SELECT * FROM app.tasks WHERE id = @taskId";
        var row = this.connection.Query(sql, new { @taskId = Guid.Parse(taskId) })
                                 .SingleOrDefault();
        if (row == null) return null;
        var task = new TaskDbDto() {
            Id = row.id.ToString(),
            Name = row.name,
            Description = row.description,
            UserId = row.user_id.ToString()
        };
        return task;
    }
}
