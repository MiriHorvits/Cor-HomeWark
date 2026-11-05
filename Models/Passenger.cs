namespace _2026_10_06.Models;

public class Passenger
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PassengerId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.Now;
}
