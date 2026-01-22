using System.Linq.Expressions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Categories;

internal sealed class CategoryQueries
{
    public static Expression<Func<Category, CategoryDto>> ProjectToDto()
    {
        return category => new()
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt
        };
    }
}
