namespace VinPocket.Api.Dtos.Categories;

public sealed record UpdateCategoryDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
};
