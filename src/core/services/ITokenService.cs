using Todos.Core.Dtos;

namespace Todos.Core.Services;

public interface ITokenService
{
    string GenerateToken(string userId);
    DecodedTokenDto DecodeToken(string token);
}
