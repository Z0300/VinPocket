using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Incomes;

public sealed class IncomeParameters
{
    [FromQuery]
    public string? Fields { get; init; }
}
