using Todos.Core.DataAccess;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

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

    public void Execute(string todoId, string authUserId)
    {
        this.ValidateTodoId(todoId);
        this.ValidateUserId(authUserId);
        this.CheckUserExists(authUserId);
        this.CheckTodoExists(todoId);
        this.SetNotDone(todoId);
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

    private void SetNotDone(string todoId)
    {
        this.todoDataAccess.SetNotDone(todoId);
    }
}

