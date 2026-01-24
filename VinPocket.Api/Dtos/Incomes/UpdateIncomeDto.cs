namespace VinPocket.Api.Dtos.Incomes;

public class UpdateIncomeDto
{
    public required string Id { get; set; }
    public required string CategoryId { get; init; }
    public decimal Amount { get; init; }
    public string? Source { get; set; }
    public DateOnly IncomeDate { get; init; }
}
