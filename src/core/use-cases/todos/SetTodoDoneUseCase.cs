using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Todos;

public class SetTodoDoneUseCase : ISetTodoDoneUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public SetTodoDoneUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(string? todoId, string? authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        var todoDb = this.FindTodo(validTodoId);
        this.CheckResourceOwnership(todoDb, validUserId);
        this.SetDone(validTodoId);
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

    private TodoDbDto FindTodo(string todoId)
    {
        var todo = this.todoDataAccess.FindById(todoId);
        if (todo == null) {
            throw new TodoNotFoundException();
        }
        return todo;
    }

    private void CheckResourceOwnership(TodoDbDto todoDb, string authUserId)
    {
        if (todoDb.UserId != authUserId) {
            throw new NotResourceOwnerException();
        }
    }

    private void SetDone(string todoId)
    {
        this.todoDataAccess.SetDone(todoId);
    }
}

