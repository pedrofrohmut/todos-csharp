namespace Todos.Core.Services;

public interface IPasswordService
{
    string HashPassword(string password);
}
