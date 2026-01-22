namespace VinPocket.Api.Models;

public class RefreshToken
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }

    public User User { get; set; } = null!;
}
