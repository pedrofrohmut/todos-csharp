using Todos.Core.DataAccess;
using Todos.Core.Entities;
using Todos.Core.Exceptions;

namespace Todos.Core.UseCases.Users;

public class VerifyUserUseCase : IVerifyUserUseCase
{
    private readonly IUserDataAccess userDataAccess;

    public VerifyUserUseCase(IUserDataAccess userDataAccess)
    {
        this.userDataAccess = userDataAccess;
    }

    public void Execute(string authUserId)
    {
        this.ValidateId(authUserId);
        this.CheckUserExists(authUserId);
    }

    private void ValidateId(string id)
    {
        User.ValidateId(id);
    }

    private void CheckUserExists(string id)
    {
        var user = this.userDataAccess.FindById(id);
        if (user == null) {
            throw new UserNotFoundException("User not found by id");
        }
    }
}
