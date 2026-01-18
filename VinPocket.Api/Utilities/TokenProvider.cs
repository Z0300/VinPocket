using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VinPocket.Api.Models;

namespace VinPocket.Api.Utilities;

public sealed class TokenProvider(IConfiguration configuration)
{
    public string Create(User user)
    {
        var secretKey = configuration.GetValue<string>("Jwt:Secret");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration.GetValue<string>("Jwt:Issuer"),
            Audience = configuration.GetValue<string>("Jwt:Audience"),
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public (string Token, DateTime ExpiresAt) GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(randomBytes);
        var expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:RefreshTokenInDays"));
        return (token, expires);
    }
}
