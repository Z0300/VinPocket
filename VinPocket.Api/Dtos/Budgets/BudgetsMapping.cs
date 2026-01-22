using VinPocket.Api.Common.Sorting;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Budgets;

public static class BudgetMappings
{
    public static readonly SortMappingDefinition<BudgetDto, Budget> SortMapping = new()
    {
        Mappings = [
            new SortMapping(nameof(BudgetDto.Id), nameof(Budget.Id)),
        new SortMapping(nameof(BudgetDto.Category), nameof(Budget.Category)),
        new SortMapping(nameof(BudgetDto.Period), nameof(Budget.Period)),
        new SortMapping(nameof(BudgetDto.StartDate), nameof(Budget.StartDate))
    ],
    };

    public static Budget ToEntity(this CreateBudgetDto dto, string userId)
    {
        return new()
        {
            Id = Budget.CreateNewId(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Period = dto.Period,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static BudgetDto ToDto(this Budget budget)
    {
        return new()
        {
            Id = budget.Id,
            Category = budget.Category?.Name,
            Amount = budget.Amount,
            Period = budget.Period,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt,
        };
    }

    public static void UpdateFromDto(this Budget budget, UpdateBudgetDto dto)
    {
        budget.Amount = dto.Amount;
        budget.Period = dto.Period;
        budget.UpdatedAt = DateTime.UtcNow;
    }
}
