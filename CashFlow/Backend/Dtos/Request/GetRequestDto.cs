using CashFlow.Models;

namespace CashFlow.Dtos.Request;

public class GetRequestDto
{
    public int Id { get; set; }
    public int UserId {get;set;} 
    public RequestType Type { get; set; }
    //Normal cash
    public double AccountBalance { get; set; }
    public double AmountBalance { get; set; }
    public double FinallBalance { get; set; }
    //Credit
    public double AccountCredit { get; set; }
    public double AmountCredit { get; set; }
    public double FinallCredit { get; set; }
}