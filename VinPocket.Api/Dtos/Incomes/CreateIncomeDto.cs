using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Incomes;

public sealed record CreateIncomeDto
{
    public required string CategoryId { get; init; }
    public decimal Amount { get; init; }
    public string? Source { get; set; }
    public DateOnly IncomeDate { get; init; }
}
