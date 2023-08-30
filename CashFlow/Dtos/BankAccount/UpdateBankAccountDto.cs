using CashFlow.Models;

namespace CashFlow.Dtos.BankAccount;

public class UpdateBankAccountDto
{
    public BankAccountType Type { get; set; }
    public string Name { get; set; } // Name should be created as user surname+bankaccountid+type
    public double Balance { get; set; }
    public double CreditBalance { get; set; }
    public DateTime UpdatedAt { get; set; }
}