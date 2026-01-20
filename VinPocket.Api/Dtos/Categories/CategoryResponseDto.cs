namespace VinPocket.Api.Dtos.Categories;

public sealed class CategoryResponseDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public DateTime CreatedAt { get; init; }
}
