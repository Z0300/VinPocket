using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Expenses;

public sealed record CreateExpenseDto
{
    public required string CategoryId { get; init; }
    public decimal Amount { get; init; }
    public Payment Payment { get; init; }
    public string? Description { get; init; }
    public DateOnly ExpenseDate { get; init; }
}
