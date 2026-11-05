namespace _2026_10_06.Models;

/// <summary>
/// הזמנה שמורה בזיכרון. BookingResponse הוא מה שמוחזר ללקוח,
/// ו-Booking הוא הרשומה המלאה (כולל מזהה הטיסה) לצורך ביטול.
/// </summary>
public class Booking
{
    public string BookingId { get; set; } = string.Empty;
    public int FlightId { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public string PassengerId { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public int PassengerCount { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }

    public BookingResponse ToResponse() => new()
    {
        BookingId = BookingId,
        FlightNumber = FlightNumber,
        PassengerName = PassengerName,
        Origin = Origin,
        Destination = Destination,
        DepartureTime = DepartureTime,
        PassengerCount = PassengerCount,
        TotalPrice = TotalPrice
    };
}
