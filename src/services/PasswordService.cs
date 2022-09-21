using Todos.Core.Services;

namespace Todos.Services;

using BCrypt.Net;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password) => BCrypt.HashPassword(password);

    public bool MatchPasswordAndHash(string password, string hash) => BCrypt.Verify(password, hash);
}
