using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Expenses;

public sealed record UpdateExpenseDto
{
    public required string Id { get; set; }
    public decimal Amount { get; init; }
    public Payment Payment { get; init; }
    public string? Description { get; init; }
    public DateOnly ExpenseDate { get; init; }
}
