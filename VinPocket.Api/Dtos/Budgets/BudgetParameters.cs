using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Budgets;

public sealed class BudgetParameters
{
    [FromQuery]
    public string? Fields { get; init; }
}
