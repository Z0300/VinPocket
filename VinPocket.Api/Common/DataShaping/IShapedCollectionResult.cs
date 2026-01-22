using System.Dynamic;

namespace VinPocket.Api.Common.DataShaping;

public interface IShapedCollectionResult
{
    public IReadOnlyCollection<ExpandoObject> Data { get; init; }
}
