using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Budgets;

public sealed record CreateBudgetDto
{
    public required string UserId { get; init; }
    public required string CategoryId { get; init; }
    public decimal Amount { get; init; }
    public PeriodType Period { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}
