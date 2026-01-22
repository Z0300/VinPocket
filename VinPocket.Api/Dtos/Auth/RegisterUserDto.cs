namespace VinPocket.Api.Dtos.Auth;

public sealed record RegisterUserDto(
    string Name,
    string Email,
    string Password);
