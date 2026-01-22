using System.Linq.Expressions;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Budgets;

internal static class BudgetQueries
{
    public static Expression<Func<Budget, BudgetDto>> ProjectToDto()
    {
        return budget => new()
        {
            Id = budget.Id,
            Category = budget.Category == null ? null : budget.Category.Name,
            Amount = budget.Amount,
            Period = budget.Period,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt,
        };
    }

    public static Expression<Func<Budget, BudgetWithCategoryDto>> ProjectToDtoWithCategory()
    {
        return budget => new()
        {
            Id = budget.Id,
            Category = budget.Category == null ? null : new()
            {
                Id = budget.Category.Id,
                Name = budget.Category.Name,
                CreatedAt = budget.Category.CreatedAt
            },
            Amount = budget.Amount,
            Period = budget.Period,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt,
        };
    }
}
