using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Expenses;

public sealed class ExpenseParameters
{
    [FromQuery(Name = "fields")]
    public string? Fields { get; init; }
}
