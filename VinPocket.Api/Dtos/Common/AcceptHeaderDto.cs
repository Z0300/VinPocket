using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Common;

public record AcceptHeaderDto
{
    [FromHeader(Name = "Accept")]
    public string? Accept { get; init; }
}
