using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Todos;

public class SetTodoNotDoneUseCase : ISetTodoNotDoneUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public SetTodoNotDoneUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(string? todoId, string? authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var todoDb = this.FindTodo(validUserId);
        this.CheckResourceOwnership(todoDb, validUserId);
        this.SetNotDone(validTodoId);
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

    private TodoDbDto FindTodo(string todoId)
    {
        var todo = this.todoDataAccess.FindById(todoId);
        if (todo == null) {
            throw new TodoNotFoundException();
        }
        return todo;
    }

    private async Task<TodoDbDto> FindTodoAsync(string todoId)
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

    private void SetNotDone(string todoId)
    {
        this.todoDataAccess.SetNotDone(todoId);
    }

    private Task SetNotDoneAsync(string todoId)
    {
        return this.todoDataAccess.SetNotDoneAsync(todoId);
    }

    public async Task ExecuteAsync(string? todoId, string? authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        var validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        var todoDb = await this.FindTodoAsync(validUserId);
        this.CheckResourceOwnership(todoDb, validUserId);
        await this.SetNotDoneAsync(validTodoId);
    }
}
