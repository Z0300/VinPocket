using VinPocket.Api.Common.Auth;
using VinPocket.Api.Models;
using BC = BCrypt.Net.BCrypt;

namespace VinPocket.Api.Dtos.Auth;

public static class AuthMappings
{
    public static User ToEntity(this RegisterUserDto dto)
    {
        return new()
        {
            Id = User.CreateNewId(),
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BC.EnhancedHashPassword(dto.Password),
            Role = Roles.Member,
            CreatedAt = DateTime.UtcNow,
        };
    }

   

}
