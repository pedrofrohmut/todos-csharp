using Todos.Core.DataAccess;
using Todos.Core.Dtos;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Todos;

public class UpdateTodoUseCase : IUpdateTodoUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public UpdateTodoUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(string todoId, UpdateTodoDto updatedTodo, string authUserId)
    {
        var validTodoId = this.ValidateTodoId(todoId);
        this.ValidateTodo(updatedTodo);
        var validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(authUserId);
        var todoDb = this.FindTodo(todoId);
        this.CheckResourceOwnership(todoDb, validUserId);
        this.UpdateTodo(todoId, updatedTodo);
    }

    private string ValidateTodoId(string? todoId)
    {
        Todo.ValidateId(todoId);
        return todoId!;
    }

    private void ValidateTodo(UpdateTodoDto updatedTodo)
    {
        Todo.ValidateName(updatedTodo.Name);
        Todo.ValidateDescription(updatedTodo.Description);
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

    private void UpdateTodo(string todoId, UpdateTodoDto updatedTodo)
    {
        this.todoDataAccess.Update(todoId, updatedTodo);
    }
}
