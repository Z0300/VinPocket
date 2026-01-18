namespace VinPocket.Api.Contracts.Users;

public sealed record UserRegistrationRequest(
    string Name,
    string Email,
    string Password);
