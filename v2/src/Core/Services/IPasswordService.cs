namespace Todos.Core.Services;

public interface IPasswordService
{
    public string HashPassword(string password);
    public bool CheckPassword(string password, string passwordHash);
}
