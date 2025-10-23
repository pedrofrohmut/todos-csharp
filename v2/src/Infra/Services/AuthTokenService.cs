using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Infra.Services;

public class AuthTokenService : IAuthTokenService
{
    public Result<AuthToken> Decode(string token)
    {
        throw new NotImplementedException();
    }
}
