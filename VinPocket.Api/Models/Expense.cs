namespace VinPocket.Api.Models;

public class Expense
{
    public required string Id { get; set; }
    public required string UserId { get; set; }
    public required string CategoryId { get; set; }
    public decimal Amount { get; set; }
    public Payment PaymentMethod { get; set; }
    public string? Description { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;

    public static string CreateNewId() => $"e_{Guid.CreateVersion7()}";

    public enum Payment
    {
        Cash = 1,
        CreditCard = 2,
        DebitCard = 3,
        EWallet = 4,
        Other = 5
    }

}
