namespace VinPocket.Api.Models;

public class Category
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public static string CreateNewId() => $"c_{Guid.CreateVersion7()}";
}
