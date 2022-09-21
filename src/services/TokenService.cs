using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
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
}
