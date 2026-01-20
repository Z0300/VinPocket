namespace VinPocket.Api.Dtos.Users;

public class UserLoginResponseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
