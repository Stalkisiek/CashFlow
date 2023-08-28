namespace CashFlow.Models;

public class User : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash = new byte[0];
    public byte[] PasswordSalt = new byte[0];
    public AuthorizationLevel AuthorizationLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<BankAccount>? BankAccounts { get; set; }
}