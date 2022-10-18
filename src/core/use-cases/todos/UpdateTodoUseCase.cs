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
        this.ValidateTodoId(todoId);
        this.ValidateTodo(updatedTodo);
        this.ValidateUserId(authUserId);
        this.CheckUserExists(authUserId);
        this.CheckTodoExists(todoId);
        this.UpdateTodo(todoId, updatedTodo);
    }

    private void ValidateTodoId(string todoId)
    {
        Todo.ValidateId(todoId);
    }

    private void ValidateTodo(UpdateTodoDto updatedTodo)
    {
        Todo.ValidateName(updatedTodo.Name);
        Todo.ValidateDescription(updatedTodo.Description);
    }

    private void ValidateUserId(string authUserId)
    {
        User.ValidateId(authUserId);
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindUserById(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private void CheckTodoExists(string todoId)
    {
        var todo = this.todoDataAccess.FindById(todoId);
        if (todo == null) {
            throw new TodoNotFoundException();
        }
    }

    private void UpdateTodo(string todoId, UpdateTodoDto updatedTodo)
    {
        this.todoDataAccess.Update(todoId, updatedTodo);
    }
}
