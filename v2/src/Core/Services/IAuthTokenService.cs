using Todos.Core.Utils;

namespace Todos.Core.Services;

public interface IAuthTokenService
{
    public Result<AuthToken> Decode(string token);
}
