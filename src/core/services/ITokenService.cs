namespace Todos.Core.Services;

public interface ITokenService
{
    string GenerateToken(string userId);
}
