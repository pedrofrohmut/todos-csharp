using Todos.Core.Services;

namespace Todos.Infra.Services;

using BCrypt.Net;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public bool CheckPassword(string password, string passwordHash)
    {
        return BCrypt.Verify(password, passwordHash);
    }
}
