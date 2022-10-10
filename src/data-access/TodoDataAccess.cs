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

    public List<TodoDbDto> FindByTaskId(string taskId)
    {
        var sql = "SELECT * FROM app.todos WHERE task_id = @taskId";
        var rows = this.connection.Query(sql, new { @taskId = Guid.Parse(taskId) });
        if (rows == null) return new List<TodoDbDto>();
        var todos = rows
            .Select(row => new TodoDbDto() {
                Id = row.id.ToString(),
                Name = row.name,
                Description = row.description,
                IsDone = row.is_done,
                TaskId = row.task_id.ToString(),
                UserId = row.user_id.ToString()
            })
            .ToList();
        return todos;
    }

    public TodoDbDto? FindById(string todoId)
    {
        var sql = "SELECT * FROM app.todos WHERE id = @todoId";
        var row = this.connection.Query(sql, new { @todoId = Guid.Parse(todoId) })
                                 .FirstOrDefault();
        if (row == null) return null;
        var todo = new TodoDbDto() {
            Id = row.id.ToString(),
            Name = row.name,
            Description = row.description,
            IsDone = row.is_done,
            TaskId = row.task_id.ToString(),
            UserId = row.user_id.ToString()
        };
        return todo;
    }
}
