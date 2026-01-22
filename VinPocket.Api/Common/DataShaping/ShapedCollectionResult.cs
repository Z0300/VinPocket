using System.Dynamic;
using System.Text.Json.Serialization;

namespace VinPocket.Api.Common.DataShaping;

public sealed class ShapedCollectionResult<T> : IShapedCollectionResult
{
    public required IReadOnlyCollection<ExpandoObject> Data { get; init; }
    [JsonIgnore] public required IReadOnlyCollection<T> OriginalData { get; init; }
}