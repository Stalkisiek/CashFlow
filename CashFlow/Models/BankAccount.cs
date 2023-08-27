namespace CashFlow.Models;

public class BankAccount : Entity
{
    public int Id { get; set; }
    public BankAccountType Type { get; set; }
    public string Name { get; set; } // Name should be created as user surname+bankaccountid+type
    public double Balance { get; set; }
    public double CreditBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public BankAccount()
    {
        Name = Type.ToString() + "_" + Id;
    }
    
}