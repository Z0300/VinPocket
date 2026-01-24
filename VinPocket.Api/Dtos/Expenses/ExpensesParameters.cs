using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Expenses;

public sealed record ExpensesParameters
{
    [FromQuery]
    public DateOnly? ExpenseDate { get; init; }
    [FromQuery]
    public string? Sort { get; init; }
    [FromQuery]
    public string? Fields { get; init; }
    [FromQuery]
    public int Page { get; init; } = 1;
    [FromQuery]
    public int PageSize { get; init; } = 10;

    public void Deconstruct(
       out DateOnly? expenseDate,
       out string? sort,
       out string? fields,
       out int page,
       out int pageSize)
    {
        expenseDate = ExpenseDate;
        sort = Sort;
        fields = Fields;
        page = Page;
        pageSize = PageSize;
    }
}
