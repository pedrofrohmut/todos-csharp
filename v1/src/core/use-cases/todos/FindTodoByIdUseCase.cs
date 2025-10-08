using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Todos;

public class FindTodoByIdUseCase : IFindTodoByIdUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public FindTodoByIdUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public TodoDto Execute(string? todoId, string? authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var todoDb = this.FindTodoById(validTodoId);
        this.CheckResourceOwnership(todoDb, validUserId);
        var todo = this.MapTodoDbToTodo(todoDb);
        return todo;
    }

    private string ValidateTodoId(string? todoId)
    {
        Todo.ValidateId(todoId);
        return todoId!;
    }

    private string ValidateUserId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindById(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private async Task CheckUserExistsAsync(string authUserId)
    {
        var user = await this.userDataAccess.FindByIdAsync(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private TodoDbDto FindTodoById(string todoId)
    {
        var todo = this.todoDataAccess.FindById(todoId);
        if (todo == null) {
            throw new TodoNotFoundException();
        }
        return todo;
    }

    private async Task<TodoDbDto> FindTodoByIdAsync(string todoId)
    {
        var todo = await this.todoDataAccess.FindByIdAsync(todoId);
        if (todo == null) {
            throw new TodoNotFoundException();
        }
        return todo;
    }

    private void CheckResourceOwnership(TodoDbDto todoDb, string authUserId)
    {
        if (todoDb.UserId.ToString() != authUserId) {
            throw new NotResourceOwnerException();
        }
    }

    private TodoDto MapTodoDbToTodo(TodoDbDto todoDb) =>
        new TodoDto() {
            Id = todoDb.Id.ToString(),
            Name = todoDb.Name,
            Description = todoDb.Description,
            IsDone = todoDb.IsDone,
            UserId = todoDb.UserId.ToString(),
            TaskId = todoDb.TaskId.ToString()
        };

    public async Task<TodoDto> ExecuteAsync(string? todoId, string? authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        var validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var todoDb = await this.FindTodoByIdAsync(validTodoId);
        this.CheckResourceOwnership(todoDb, validUserId);
        var todo = this.MapTodoDbToTodo(todoDb);
        return todo;
    }
}
