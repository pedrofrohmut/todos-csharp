using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Infra.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly byte[] secretKey;

    public AuthTokenService(byte[] secretKey)
    {
        this.secretKey = secretKey;
    }

    public Result<AuthToken> Decode(string token)
    {
        throw new NotImplementedException();
    }

    public Result<string> GenerateJWT(int userId)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        // long expiration = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
        long expiration = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds();
        var payload = new Dictionary<string, object> {
            { "userId", userId },
            { "iat", now },
            { "exp", expiration },
        };

        string token = Jose.JWT.Encode(payload, secretKey, "HS256");

        return Result<string>.Ok(token);
    }
}
