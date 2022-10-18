using Todos.Core.DataAccess;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Todos;

public class DeleteCompletedTodosUseCase : IDeleteCompletedTodosUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public DeleteCompletedTodosUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(string? authUserId)
    {
        string validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        this.DeleteCompletedTodos(validUserId);
    }

    private string ValidateUserId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindUserById(authUserId);
        if (user == null) {
            throw new UserNotFoundException();
        }
    }

    private void DeleteCompletedTodos(string authUserId)
    {
        this.todoDataAccess.DeleteCompleted(authUserId);
    }
}
