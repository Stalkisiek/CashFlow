using CashFlow.Models;

namespace CashFlow.Dtos.Account;


public class GetUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AuthorizationLevel AuthorizationLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}