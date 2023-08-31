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
    [Column(TypeName = "decimal(18, 2)")]
    public double AccountBalance { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public double AmountBalance { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public double FinallBalance { get; set; }
    //Credit
    [Column(TypeName = "decimal(18, 2)")]
    public double AccountCredit { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public double AmountCredit { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public double FinallCredit { get; set; }
    
    public User? User { get; set; }
}