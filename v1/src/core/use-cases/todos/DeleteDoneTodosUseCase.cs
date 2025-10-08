using Todos.Core.DataAccess;
using Todos.Core.Entities;
using Todos.Core.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace Todos.Core.UseCases.Todos;

public class DeleteDoneTodosUseCase : IDeleteDoneTodosUseCase
{
    private readonly IUserDataAccess userDataAccess;
    private readonly ITodoDataAccess todoDataAccess;

    public DeleteDoneTodosUseCase(IUserDataAccess userDataAccess, ITodoDataAccess todoDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.todoDataAccess = todoDataAccess;
    }

    public void Execute(string? authUserId)
    {
        string validUserId = this.ValidateUserId(authUserId);
        this.CheckUserExists(validUserId);
        this.DeleteDoneTodos(validUserId);
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

    private void DeleteDoneTodos(string authUserId)
    {
        this.todoDataAccess.DeleteDone(authUserId);
    }

    private Task DeleteDoneTodosAsync(string authUserId)
    {
        return this.todoDataAccess.DeleteDoneAsync(authUserId);
    }

    public async Task ExecuteAsync(string? authUserId)
    {
        string validUserId = this.ValidateUserId(authUserId);
        await this.CheckUserExistsAsync(validUserId);
        await this.DeleteDoneTodosAsync(validUserId);
    }
}
