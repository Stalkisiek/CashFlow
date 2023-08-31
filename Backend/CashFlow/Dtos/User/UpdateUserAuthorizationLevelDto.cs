using CashFlow.Models;

namespace CashFlow.Dtos.User;

public class UpdateUserAuthorizationLevelDto
{
    public int Id { get; set; }
    public AuthorizationLevel AuthorizationLevel { get; set; }
}