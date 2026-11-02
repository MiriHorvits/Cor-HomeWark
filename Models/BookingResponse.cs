namespace _2026_10_06.Models;

public class BookingResponse
{
    public string BookingId { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public int PassengerCount { get; set; }
    public decimal TotalPrice { get; set; }
}
