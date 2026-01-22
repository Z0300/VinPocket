using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Categories;

public sealed record CategoriesParameters
{
    [FromQuery(Name = "sort")]
    public string? Sort { get; init; }
    [FromQuery(Name = "fields")]
    public string? Fields { get; init; }
    [FromQuery(Name = "page")]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; init; } = 10;

    public void Deconstruct(
        out string? sort,
        out string? fields,
        out int page,
        out int pageSize)
    {
        sort = Sort;
        fields = Fields;
        page = Page;
        pageSize = PageSize;
    }
}
