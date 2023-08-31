using CashFlow.Models;

namespace CashFlow.Dtos.Request;

public class AddRequestDto
{
    public RequestType Type { get; set; }
    public int AccountId { get; set; }
    //Normal cash
    public double? AmountBalance { get; set; }
    //Credit
    public double? AmountCredit { get; set; }

}