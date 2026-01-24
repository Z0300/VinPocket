using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Expenses;

public sealed record ExpenseDto
{
    public required string Id { get; init; }
    public string? Category { get; init; }
    public decimal Amount { get; init; }
    public Payment Payment { get; init; }
    public string? Description { get; init; }
    public DateOnly ExpenseDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
