using System.Data;
using Dapper;
using Todos.Core.DataAccess;
using Todos.Core.Dtos;

namespace Todos.DataAccess;

public class TodoDataAccess : ITodoDataAccess
{
    private readonly IDbConnection connection;

    public TodoDataAccess(IDbConnection connection)
    {
        this.connection = connection;
    }

    public void Create(CreateTodoDto newTodo, string userId)
    {
        var sql = @"INSERT INTO app.todos (name, description, is_done, task_id, user_id)
                    VALUES (@name, @description, @isDone, @taskId, @userId)";
        this.connection.Query(sql, new {
            @name = newTodo.Name,
            @description = newTodo.Description,
            @isDone = newTodo.IsDone,
            @taskId = Guid.Parse(newTodo.TaskId),
            @userId = Guid.Parse(userId)
        });
    }

    public List<TodoDbDto>? FindByTaskId(string taskId)
    {
        var sql = @"SELECT id, name, description, is_done as isDone, task_id as taskId, user_id as userId
                    FROM app.todos
                    WHERE task_id = @taskId";
        var todos = this.connection.Query<TodoDbDto>(sql, new { @taskId = Guid.Parse(taskId) })
                                   .ToList();
        return todos;
    }

    public TodoDbDto? FindById(string todoId)
    {
        var sql = @"SELECT id, name, description, is_done as isDone, task_id as taskId, user_id as userId
                    FROM app.todos
                    WHERE id = @todoId";
        var todo = this.connection.Query<TodoDbDto>(sql, new { @todoId = Guid.Parse(todoId) })
                                  .FirstOrDefault();
        return todo;
    }

    public void SetDone(string todoId)
    {
        var sql = "UPDATE app.todos SET is_done = true WHERE id = @todoId";
        this.connection.Query(sql, new { @todoId = Guid.Parse(todoId) });
    }

    public void SetNotDone(string todoId)
    {
        var sql = "UPDATE app.todos SET is_done = false WHERE id = @todoId";
        this.connection.Query(sql, new { @todoId = Guid.Parse(todoId) });
    }


    public void Update(string todoId, UpdateTodoDto updatedTodo)
    {
        var sql = @"UPDATE app.todos
                    SET name = @name, description = @description, is_done = @isDone
                    WHERE id = @todoId";
        this.connection.Query(sql, new {
            @todoId = Guid.Parse(todoId),
            @name = updatedTodo.Name,
            @description = updatedTodo.Description,
            @isDone = updatedTodo.IsDone
        });
    }

    public void Delete(string todoId)
    {
        var sql = "DELETE FROM app.todos WHERE id = @todoId";
        this.connection.Query(sql, new { @todoId = Guid.Parse(todoId) });
    }

    public void DeleteDone(string userId)
    {
        var sql = "DELETE FROM app.todos WHERE is_done = true AND user_id = @userId";
        this.connection.Query(sql, new { @userId = Guid.Parse(userId) });
    }

    public void DeleteDoneByTaskId(string taskId)
    {
        var sql = "DELETE FROM app.todos WHERE is_done = true AND task_id = @taskId";
        this.connection.Query(sql, new { @taskId = Guid.Parse(taskId) });
    }
}
