using System.Dynamic;

namespace VinPocket.Api.Common.DataShaping;

public sealed record ShapedResult<T>
{
    public required ExpandoObject Item { get; init; }
    public required T OriginalItem { get; init; }
}
