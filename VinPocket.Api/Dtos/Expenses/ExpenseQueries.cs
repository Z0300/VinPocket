using System.Linq.Expressions;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Expenses;

internal static class ExpenseQueries
{
    public static Expression<Func<Expense, ExpenseDto>> ProjectToDto()
    {
        return expense => new()
        {
            Id = expense.Id,
            Category = expense.Category == null ? null : expense.Category.Name,
            Amount = expense.Amount,
            Payment = expense.PaymentMethod,
            Description = expense.Description,
            ExpenseDate = expense.ExpenseDate,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
        };
    }

    public static Expression<Func<Expense, ExpenseWithCategoryDto>> ProjectToDtoWithCategory()
    {
        return expense => new()
        {
            Id = expense.Id,
            Category = expense.Category == null ? null : new()
            {
                Id = expense.Category.Id,
                Name = expense.Category.Name,
                CreatedAt = expense.Category.CreatedAt
            },
            Amount = expense.Amount,
            Payment = expense.PaymentMethod,
            Description = expense.Description,
            ExpenseDate = expense.ExpenseDate,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
        };
    }
}
