using VinPocket.Api.Dtos.Categories;

namespace VinPocket.Api.Dtos.Incomes;

public class IncomeWithCategoryDto
{
    public required string Id { get; init; }
    public CategoryDto? Category { get; init; }
    public decimal Amount { get; init; }
    public string? Source { get; set; }
    public DateOnly IncomeDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
