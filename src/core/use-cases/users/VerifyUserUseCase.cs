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

    public void Execute(string? authUserId)
    {
        var validUserId = this.ValidateId(authUserId);
        this.CheckUserExists(validUserId);
    }

    private string ValidateId(string? authUserId)
    {
        User.ValidateId(authUserId);
        return authUserId!;
    }

    private void CheckUserExists(string authUserId)
    {
        var user = this.userDataAccess.FindById(authUserId);
        if (user == null) {
            throw new UserNotFoundException("User not found by id");
        }
    }
}
