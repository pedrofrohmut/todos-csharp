using Todos.Core.Exceptions;
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

    [Obsolete("Use GetUserIdFromRequest instead")]
    public static string GetUserIdFromToken(HttpRequest req, ITokenService tokenService)
    {
        var token = ControllerUtils.GetToken(req);
        var decoded = tokenService.DecodeToken(token);
        if (String.IsNullOrWhiteSpace(decoded.UserId)) {
            throw new ArgumentException("The token does not have a valid userId");
        }
        return decoded.UserId;
    }

    [Obsolete("Get authUserId from HttpContext instead")]
    public static string GetUserIdFromRequest(HttpRequest req, ITokenService tokenService)
    {
        try {
            var token = ControllerUtils.GetToken(req);
            var decoded = tokenService.DecodeToken(token);
            if (String.IsNullOrWhiteSpace(decoded.UserId)) {
                throw new InvalidRequestAuthException("The token does not have a valid userId");
            }
            return decoded.UserId;
        } catch (Exception e) {
            if (e is ArgumentException) {
                throw new InvalidRequestAuthException();
            }
            throw new InvalidRequestAuthException(e.Message);
        }
        
    }
}
