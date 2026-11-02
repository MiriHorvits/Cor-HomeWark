namespace _2026_10_06.Models;

public class BookingRequest
{
    public int FlightId { get; set; }
    public string PassengerName { get; set; } = string.Empty;
    public string PassengerId { get; set; } = string.Empty;
    public int PassengerCount { get; set; } = 1;
}
