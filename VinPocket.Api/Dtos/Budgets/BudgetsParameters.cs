using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Budgets;

public sealed record BudgetsParameters
{
    [FromQuery]
    public DateOnly? StartDate { get; init; }
    [FromQuery]
    public DateOnly? EndDate { get; init; }
    [FromQuery]
    public string? Sort { get; init; }
    [FromQuery]
    public string? Fields { get; init; }
    [FromQuery]
    public int Page { get; init; } = 1;
    [FromQuery]
    public int PageSize { get; init; } = 10;

    public void Deconstruct(
        out DateOnly? startDate,
        out DateOnly? endDate,
        out string? sort,
        out string? fields,
        out int page,
        out int pageSize)
    {
        startDate = StartDate;
        endDate = EndDate;
        sort = Sort;
        fields = Fields;
        page = Page;
        pageSize = PageSize;
    }
}
