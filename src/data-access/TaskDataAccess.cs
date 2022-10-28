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

    public Task CreateAsync(CreateTaskDto newTask, string userId)
    {
        var sql = @"INSERT INTO app.tasks (name, description, user_id)
                    VALUES (@name, @description, @userId)";
        return this.connection.QueryAsync(sql, new {
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

    public Task DeleteAsync(string taskId)
    {
        var sql = "DELETE FROM app.tasks WHERE id = @taskId";
        return this.connection.QueryAsync(sql, new { @taskId = Guid.Parse(taskId) });
    }

    public TaskDbDto? FindById(string taskId)
    {
        var sql = @"SELECT id, name, description, user_id as userId
                    FROM app.tasks
                    WHERE id = @taskId";
        var task = this.connection
            .QueryFirstOrDefault<TaskDbDto>(sql, new { @taskId = Guid.Parse(taskId) });
        return task;
    }

    public async Task<TaskDbDto?> FindByIdAsync(string taskId)
    {
        var sql = @"SELECT id, name, description, user_id as userId
                    FROM app.tasks
                    WHERE id = @taskId";
        var task = await this.connection
            .QueryFirstOrDefaultAsync<TaskDbDto>(sql, new { @taskId = Guid.Parse(taskId) });
        return task;
    }

    public List<TaskDbDto> FindByUserId(string userId)
    {
        var sql = @"SELECT id, name, description, user_id as userId
                    FROM app.tasks
                    WHERE user_id = @userId";
        var tasks = this.connection.Query<TaskDbDto>(sql, new { @userId = Guid.Parse(userId) })
                                   .ToList();
        return tasks;
    }

    public async Task<List<TaskDbDto>> FindByUserIdAsync(string userId)
    {
        var sql = @"SELECT id, name, description, user_id as userId
                    FROM app.tasks
                    WHERE user_id = @userId";
        var query = await this.connection.QueryAsync<TaskDbDto>(sql, new { @userId = Guid.Parse(userId) });
        var tasks = query.ToList();
        return tasks;
    }

    public void Update(string taskId, UpdateTaskDto updatedTask)
    {
        var sql = "UPDATE app.tasks SET name = @name, description = @description WHERE id = @id";
        this.connection.Query(sql, new {
            @name = updatedTask.Name,
            @description = updatedTask.Description,
            @id = Guid.Parse(taskId)
        });
    }

    public Task UpdateAsync(string taskId, UpdateTaskDto updatedTask)
    {
        var sql = "UPDATE app.tasks SET name = @name, description = @description WHERE id = @id";
        return this.connection.QueryAsync(sql, new {
            @name = updatedTask.Name,
            @description = updatedTask.Description,
            @id = Guid.Parse(taskId)
        });
    }
}
