using CashFlow.Models;

namespace CashFlow.Dtos.Request;

public class GetPreviousRequestDto
{
    public int Id { get; set; }
    public int UserId {get;set;}
    public int RequestId { get; set; }
    public RequestType Type { get; set; }
    public RequestAcceptMode Status { get; set; }
}