namespace VinPocket.Api.Dtos.Users;

public sealed record UserRegistrationRequestDto(
    string Name,
    string Email,
    string Password);
