namespace VinPocket.Api.Contracts.Users;

public class UserLoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
