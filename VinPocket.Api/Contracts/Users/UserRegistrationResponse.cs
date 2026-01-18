namespace VinPocket.Api.Contracts.Users;

public class UserRegistrationResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public required string Email { get; set; }
}
