using VinPocket.Api.Common.Sorting;
using VinPocket.Api.Dtos.Budgets;
using VinPocket.Api.Models;

namespace VinPocket.Api.Dtos.Incomes;

public static class IncomeMappings
{
    public static readonly SortMappingDefinition<IncomeDto, Income> SortMapping = new()
    {
        Mappings = [
            new SortMapping(nameof(IncomeDto.Id), nameof(Income.Id)),
        new SortMapping(nameof(IncomeDto.Category), nameof(Income.Category)),
        new SortMapping(nameof(IncomeDto.Source), nameof(Income.Source)),
        new SortMapping(nameof(IncomeDto.IncomeDate), nameof(Income.IncomeDate))
    ],
    };

    public static Income ToEntity(this CreateIncomeDto dto, string userId)
    {
        return new()
        {
            Id = Income.CreateNewId(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Source = dto.Source,
            IncomeDate = dto.IncomeDate,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static IncomeDto ToDto(this Income income)
    {
        return new()
        {
            Id = income.Id,
            Category = income.Category?.Name,
            Amount = income.Amount,
            Source = income.Source,
            IncomeDate = income.IncomeDate,
            CreatedAt = income.CreatedAt,
            UpdatedAt = income.UpdatedAt,
        };
    }

    public static void UpdateFromDto(this Income income, UpdateIncomeDto dto)
    {
        income.Amount = dto.Amount;
        income.Source = dto.Source;
        income.UpdatedAt = DateTime.UtcNow;
    }
}
