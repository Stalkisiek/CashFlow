namespace CashFlow.Dtos.Authorization;

public class UpdateUserPasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}