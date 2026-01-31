using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Expenses;

public sealed record ExpensesParameters
{
    [FromQuery(Name = "expense_date")]
    public DateOnly? ExpenseDate { get; init; }


    [FromQuery(Name = "sort")]
    public string? Sort { get; init; }


    [FromQuery(Name = "fields")]
    public string? Fields { get; init; }


    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;


    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10;

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
