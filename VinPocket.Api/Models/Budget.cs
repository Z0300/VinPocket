
namespace VinPocket.Api.Models;

public sealed class Budget
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string CategoryId { get; set; }
    public decimal Amount { get; set; }
    public PeriodType Period { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }
    public Category? Category { get; set; }

    public static string CreateNewId() => $"b_{Guid.CreateVersion7()}";
}

public enum PeriodType
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}
