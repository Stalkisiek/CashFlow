using CashFlow.Models;

namespace CashFlow.Dtos.Request;

public class AddPreviousRequestDto
{
    public int RequestId { get; set; }
    public int UserId { get; set; }
    public RequestType Type { get; set; }
    public RequestAcceptMode Status { get; set; }
}