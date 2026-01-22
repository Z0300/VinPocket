namespace VinPocket.Api.Models;

public class User
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }

    public static string CreateNewId() => $"u_{Guid.CreateVersion7()}";
}
