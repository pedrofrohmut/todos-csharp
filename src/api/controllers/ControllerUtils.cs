using Todos.Core.Services;

namespace Todos.Api.Controllers;

public static class ControllerUtils
{
    public static string GetToken(HttpRequest req)
    {
        if (req.Headers.TryGetValue("Authorization", out var auth)) {
            var token = auth.ToString().Split(" ")[1];
            return token;
        } else {
            throw new ArgumentException("Could not get authorization for this request headers");
        }
    }

    public static string GetUserIdFromToken(HttpRequest req, ITokenService tokenService)
    {
        var token = ControllerUtils.GetToken(req);
        var decoded = tokenService.DecodeToken(token);
        if (String.IsNullOrWhiteSpace(decoded.UserId)) {
            throw new ArgumentException("The token does not have a valid userId");
        }
        return decoded.UserId;
    }
}
