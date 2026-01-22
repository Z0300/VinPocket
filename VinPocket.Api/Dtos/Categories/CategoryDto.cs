namespace VinPocket.Api.Dtos.Categories;

public sealed class CategoryDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public DateTime CreatedAt { get; init; }
}
