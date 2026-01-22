using System.Dynamic;
using System.Text.Json.Serialization;
using VinPocket.Api.Common.Pagination;

namespace VinPocket.Api.Common.DataShaping;

public sealed record ShapedPaginationResult<T> : IShapedCollectionResult, IPaginationResult
{
    public required IReadOnlyCollection<ExpandoObject> Data { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public long TotalCount { get; init; }
    public long TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}