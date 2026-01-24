using VinPocket.Api.Common.Sorting;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Expenses;

public static class ExpensesMappings
{
    public static readonly SortMappingDefinition<ExpenseDto, Expense> SortMapping = new()
    {
        Mappings = [
            new SortMapping(nameof(ExpenseDto.Id), nameof(Expense.Id)),
        new SortMapping(nameof(ExpenseDto.Category), nameof(Expense.Category)),
        new SortMapping(nameof(ExpenseDto.Payment), nameof(Expense.PaymentMethod)),
        new SortMapping(nameof(ExpenseDto.ExpenseDate), nameof(Expense.ExpenseDate))
    ],
    };

    public static Expense ToEntity(this CreateExpenseDto dto, string userId)
    {
        return new()
        {
            Id = Expense.CreateNewId(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            PaymentMethod = dto.Payment,
            Description = dto.Description,
            ExpenseDate = dto.ExpenseDate,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static ExpenseDto ToDto(this Expense expense)
    {
        return new()
        {
            Id = expense.Id,
            Category = expense.Category?.Name,
            Amount = expense.Amount,
            Payment = expense.PaymentMethod,
            Description = expense.Description,
            ExpenseDate = expense.ExpenseDate,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
        };
    }

    public static void UpdateFromDto(this Expense expense, UpdateExpenseDto dto)
    {
        expense.Amount = dto.Amount;
        expense.PaymentMethod = dto.Payment;
        expense.Description = dto.Description;
        expense.ExpenseDate = dto.ExpenseDate;
        expense.UpdatedAt = DateTime.UtcNow;
    }
}
