using Todos.Core.Services;

namespace Todos.Infra.Services;

public class PasswordService : IPasswordService
{
    public bool CheckPassword(string password, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public string HashPassword(string password)
    {
        throw new NotImplementedException();
    }
}
