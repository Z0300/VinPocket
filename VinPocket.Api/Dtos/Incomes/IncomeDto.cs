namespace VinPocket.Api.Dtos.Incomes;

public sealed record IncomeDto
{
    public required string Id { get; init; }
    public string? Category { get; init; }
    public decimal Amount { get; init; }
    public string? Source { get; set; }
    public DateOnly IncomeDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
