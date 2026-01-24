using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Incomes;

public sealed record IncomesParameters
{
    [FromQuery]
    public DateOnly? IncomeDate { get; init; }
    [FromQuery]
    public string? Sort { get; init; }
    [FromQuery]
    public string? Fields { get; init; }
    [FromQuery]
    public int Page { get; init; } = 1;
    [FromQuery]
    public int PageSize { get; init; } = 10;

    public void Deconstruct(
        out DateOnly? incomeDate,
        out string? sort,
        out string? fields,
        out int page,
        out int pageSize)
    {
        incomeDate = IncomeDate;
        sort = Sort;
        fields = Fields;
        page = Page;
        pageSize = PageSize;
    }
}
