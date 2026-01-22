using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VinPocket.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? identityId = (claimsPrincipal?
            .FindFirstValue(JwtRegisteredClaimNames.Sub)) ?? 
            throw new ApplicationException("User identifier is unavailable");

        return identityId;
    }
}
