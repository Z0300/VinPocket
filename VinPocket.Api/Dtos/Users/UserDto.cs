namespace VinPocket.Api.Dtos.Users;

public sealed record UserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
