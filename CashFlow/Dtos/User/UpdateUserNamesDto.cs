namespace CashFlow.Dtos.User;

public class UpdateUserNamesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}