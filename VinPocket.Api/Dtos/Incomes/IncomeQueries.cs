using System.Linq.Expressions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Incomes;

internal static class IncomeQueries
{
    public static Expression<Func<Income, IncomeDto>> ProjectToDto()
    {
        return income => new()
        {
            Id = income.Id,
            Category = income.Category == null ? null : income.Category.Name,
            Amount = income.Amount,
            Source = income.Source,
            IncomeDate = income.IncomeDate,
            CreatedAt = income.CreatedAt,
            UpdatedAt = income.UpdatedAt,
        };
    }

    public static Expression<Func<Income, IncomeWithCategoryDto>> ProjectToDtoWithCategory()
    {
        return income => new()
        {
            Id = income.Id,
            Category = income.Category == null ? null : new()
            {
                Id = income.Category.Id,
                Name = income.Category.Name,
                CreatedAt = income.Category.CreatedAt
            },
            Amount = income.Amount,
            Source = income.Source,
            IncomeDate = income.IncomeDate,
            CreatedAt = income.CreatedAt,
            UpdatedAt = income.UpdatedAt,
        };
    }
}
