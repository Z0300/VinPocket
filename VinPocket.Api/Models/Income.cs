namespace VinPocket.Api.Models;

public class Income
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Source { get; set; }
    public DateOnly IncomeDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;

    public static string CreateNewId() => $"i_{Guid.CreateVersion7()}";
}
