using Microsoft.AspNetCore.Mvc;

namespace VinPocket.Api.Dtos.Categories;

public sealed record CategoryParameters
{
    [FromQuery]
   public string? Fields { get; init; }
}
