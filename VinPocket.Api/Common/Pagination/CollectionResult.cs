namespace VinPocket.Api.Common.Pagination;

public sealed record CollectionResult<T> : ICollectionResult<T>
{
    public required IReadOnlyCollection<T> Data { get; init; }
}
