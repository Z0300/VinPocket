using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Expenses;

public sealed class ExpenseParameters
{
    [FromQuery]
    public string? Fields { get; init; }
}
