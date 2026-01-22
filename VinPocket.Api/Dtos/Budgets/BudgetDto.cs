using VinPocket.Api.Dtos.Categories;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Budgets;

public sealed record BudgetDto
{
    public required string Id { get; init; }
    public string? Category { get; init; }
    public decimal Amount { get; init; }
    public PeriodType Period { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
