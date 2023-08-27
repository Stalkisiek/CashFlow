namespace CashFlow.Models;

public class User : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash = Array.Empty<byte>();
    public byte[] PasswordSalt = Array.Empty<byte>();
    public AuthorizationLevel AuthorizationLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}