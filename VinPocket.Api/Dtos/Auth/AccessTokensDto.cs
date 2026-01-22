namespace VinPocket.Api.Dtos.Auth;

public sealed record AccessTokensDto(
    string AccessToken,
    string RefreshToken);