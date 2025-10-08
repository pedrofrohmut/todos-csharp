namespace Todos.Core.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool MatchPasswordAndHash(string password, string hash);
}
