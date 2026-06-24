using System.Text.Json;
using Todos.Core.Services;
using Todos.Core.Utils;

namespace Todos.Infra.Services;

public class AuthTokenService : IAuthTokenService
{
    private class JwtPayload
    {
        public int userId { get; set; }
        public long exp { get; set; }
        public long iat { get; set; }
    }

    private readonly byte[] secretKey;

    public AuthTokenService(byte[] secretKey)
    {
        this.secretKey = secretKey;
    }

    public Result<AuthToken> Decode(string token)
    {
        var failedResult = Result<AuthToken>.Fail(
            "AuthTokenService:" + nameof(Decode), "Token verification failed. The token is invalid.");

        var verified = Jose.JWT.Verify(token, secretKey, "HS256");
        if (String.IsNullOrWhiteSpace(verified)) {
            return failedResult;
        }

        var payload = JsonSerializer.Deserialize<JwtPayload>(verified);
        if (payload == null) {
            return failedResult;
        }

        var authToken = new AuthToken {
            UserId = payload.userId,
            Expiration = payload.exp,
            IssuedAt = payload.iat,
        };
        return Result<AuthToken>.Ok(authToken);
    }

    public Result<string> GenerateJWT(int userId)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expiration = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
        // long expiration = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds();
        var payload = new Dictionary<string, object> {
            { "userId", userId },
            { "iat", now },
            { "exp", expiration },
        };

        string token = Jose.JWT.Encode(payload, secretKey, "HS256");

        return Result<string>.Ok(token);
    }
}
