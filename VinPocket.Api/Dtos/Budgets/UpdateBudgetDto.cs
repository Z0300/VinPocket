using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Budgets;

public class UpdateBudgetDto
{
    public required string Id { get; init; }
    public decimal Amount { get; init; }
    public PeriodType Period { get; init; }
}
