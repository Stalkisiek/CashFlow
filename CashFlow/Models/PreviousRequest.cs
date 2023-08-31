using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Models;

public class PreviousRequest
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public RequestType Type { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public RequestAcceptMode Status { get; set; } = RequestAcceptMode.Pending;
    public int UserId {get;set;}
    public string Message { get; set; } = string.Empty;
    public User? User { get; set; }
}