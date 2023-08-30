using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Models;

public class Request
{
    public int Id { get; set; }
    public int UserId {get;set;}
    public int AccountId { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public RequestType Type { get; set; }
    //Normal cash
    public double AccountBalance { get; set; }
    public double AmountBalance { get; set; }
    public double FinallBalance { get; set; }
    //Credit
    public double AccountCredit { get; set; }
    public double AmountCredit { get; set; }
    public double FinallCredit { get; set; }
    
    public User? User { get; set; }
}