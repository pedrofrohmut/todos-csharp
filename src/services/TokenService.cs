using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Todos.Core.Dtos;
using Todos.Core.Services;

namespace Todos.Services;

public class TokenService : ITokenService
{
    private readonly string jwtSecret;

    public TokenService(string jwtSecret)
    {
        this.jwtSecret = jwtSecret;
    }

    public string GenerateToken(string userId)
    {
        var subject = new ClaimsIdentity(new Claim[] {
            new Claim("userId", userId)
        });
        // Must be Utc time
        var expirationDate = DateTime.UtcNow.AddDays(30);
        var key = Encoding.ASCII.GetBytes(this.jwtSecret);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor() {
            Subject = subject,
            Expires = expirationDate,
            SigningCredentials = credentials
        };
        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(descriptor);
        var token = handler.WriteToken(securityToken);
        return token;
    }
    
    public DecodedTokenDto DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSecret);
        var parameters = new TokenValidationParameters() {
            IssuerSigningKey         = new SymmetricSecurityKey(key),
            ValidateAudience         = false,
            ValidateIssuer           = false,
            ValidateIssuerSigningKey = true,
        };
        SecurityToken? securityToken = null;
        try {
            handler.ValidateToken(token, parameters, out securityToken);
        } catch (Exception e) {
            if (e is SecurityTokenExpiredException ||
                e is SecurityTokenInvalidSignatureException)
            {
                throw new ArgumentException("Token is expired or could not be parsed");
            }
            throw;
        }
        var decoded = securityToken as JwtSecurityToken;
        // Keys: userId, nbf, exp, iat
        var claims = decoded!.Claims .ToDictionary(claim => claim.Type, 
                                                   claim => claim.Value);
        if (!claims.ContainsKey("userId") || !claims.ContainsKey("exp")) {
            throw new ArgumentException("Invalid token. Missing required fields");
        }
        return new DecodedTokenDto() { UserId = claims["userId"] };
    }
}
