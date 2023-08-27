namespace CashFlow.Models;

public class Request
{
    public int Id { get; set; }
    public RequestType Type { get; set; }
    //Normal cash
    public double AccountBalance { get; set; }
    public double AmountBalance { get; set; }
    public double FinallBalance { get; set; }
    //Credit
    public double AccountCredit { get; set; }
    public double AmountCredit { get; set; }
    public double FinallCredit { get; set; }
    //public int userId {get;set;} // will be added after relation ships will be created
    
}