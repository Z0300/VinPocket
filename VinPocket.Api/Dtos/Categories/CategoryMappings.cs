using VinPocket.Api.Common.Sorting;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Categories;

public static class CategoryMappings
{
    public static readonly SortMappingDefinition<CategoryDto, Category> SortMapping = new()
    {
        Mappings = [
           new SortMapping(nameof(CategoryDto.Id), nameof(Category.Id)),
            new SortMapping(nameof(CategoryDto.Name), nameof(Category.Name)),
            new SortMapping(nameof(CategoryDto.CreatedAt), nameof(Category.CreatedAt)),
        ],
    };

    public static Category ToEntity(this CreateCategoryDto dto)
    {
        return new()
        {
            Id = Category.CreateNewId(),
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static CategoryDto ToDto(this Category category)
    {
        return new()
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
        };
    }

    public static void UpdateFromDto(this Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
    }
}
