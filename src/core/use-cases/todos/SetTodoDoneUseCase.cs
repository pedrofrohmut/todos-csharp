using Todos.Core.DataAccess;
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

    public void Execute(string todoId, string authUserId)
    {
        this.ValidateTodoId(todoId);
        this.ValidateUserId(todoId);
        this.CheckUserExists(authUserId);
        this.CheckTodoExists(todoId);
        this.SetDone(todoId);
    }

    private void ValidateTodoId(string todoId)
    {
        Todo.ValidateId(todoId);
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

    private void SetDone(string todoId)
    {
        this.todoDataAccess.SetDone(todoId);
    }
}

