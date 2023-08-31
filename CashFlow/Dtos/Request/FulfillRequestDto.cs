namespace CashFlow.Dtos.Request;

public class FulfillRequestDto
{
    public int Id { get; set; }
    public bool Accepted { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}