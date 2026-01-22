using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VinPocket.Api.Common.Auth;
using VinPocket.Api.Configurations;
using VinPocket.Api.Dtos.Auth;
using VinPocket.Api.Models;

namespace VinPocket.Api.Utilities;

public sealed class TokenProvider(IOptions<JwtAuthOptions> options)
{
    private readonly JwtAuthOptions _jwtAuthOptions = options.Value;
    public AccessTokensDto Create(User user)
    {
        return new(GenerateToken(user), GenerateRefreshToken());
    }

    public string GenerateToken(User user)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtAuthOptions.Key!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
       [
           new(JwtRegisteredClaimNames.Sub, user.Id),
           new(JwtRegisteredClaimNames.Email, user.Email),
           new Claim(JwtCustomClaimNames.Role, user.Role)
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtAuthOptions.Issuer,
            Audience = _jwtAuthOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(_jwtAuthOptions.ExpirationInMinutes),
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    private static string GenerateRefreshToken()
    {
        byte[] guidBytes = Encoding.UTF8.GetBytes(Guid.CreateVersion7().ToString());
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String([.. guidBytes, .. randomBytes]);
    }
}
