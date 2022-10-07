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
}
