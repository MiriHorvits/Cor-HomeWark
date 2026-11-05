namespace _2026_10_06.Models;

/// <summary>
/// מאגר נתונים בזיכרון המשותף לכל ה-controllers — טיסות והזמנות.
/// מוזרק כ-Singleton, כדי שביטול הזמנה מ-BookingsController יחזיר מקומות
/// לאותה טיסה שנקראה ב-FlightsController.
/// אין כאן נעילה מלאה — מתאים לסביבת לימוד, לא לפרודקשן.
/// </summary>
public class FlightStore
{
    public List<Flight> Flights { get; } = new()
    {
        new Flight
        {
            Id = 1,
            Airline = "El Al",
            FlightNumber = "LY 101",
            Origin = "TLV",
            Destination = "LHR",
            DepartureTime = new DateTime(2026, 11, 15, 18, 30, 0),
            ArrivalTime = new DateTime(2026, 11, 15, 20, 45, 0),
            Price = 349.90m,
            AvailableSeats = 22
        },
        new Flight
        {
            Id = 2,
            Airline = "Turkish Airlines",
            FlightNumber = "TK 205",
            Origin = "TLV",
            Destination = "IST",
            DepartureTime = new DateTime(2026, 11, 16, 8, 15, 0),
            ArrivalTime = new DateTime(2026, 11, 16, 10, 30, 0),
            Price = 289.50m,
            AvailableSeats = 18
        },
        new Flight
        {
            Id = 3,
            Airline = "Delta",
            FlightNumber = "DL 441",
            Origin = "JFK",
            Destination = "LAX",
            DepartureTime = new DateTime(2026, 11, 20, 14, 0, 0),
            ArrivalTime = new DateTime(2026, 11, 20, 17, 25, 0),
            Price = 520.00m,
            AvailableSeats = 12
        },
        new Flight
        {
            Id = 4,
            Airline = "Air France",
            FlightNumber = "AF 886",
            Origin = "CDG",
            Destination = "JFK",
            DepartureTime = new DateTime(2026, 11, 19, 21, 10, 0),
            ArrivalTime = new DateTime(2026, 11, 19, 23, 40, 0),
            Price = 410.75m,
            AvailableSeats = 9
        }
    };

    public List<Booking> Bookings { get; } = new();

    public List<Passenger> Passengers { get; } = new();

    public Flight? FindFlight(int id) => Flights.FirstOrDefault(f => f.Id == id);

    /// <summary>מייצר מזהה הזמנה של 8 תווים אקראיים — ייחודי מול הקיימים.</summary>
    public string NewBookingId()
    {
        string id;
        do
        {
            id = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        }
        while (Bookings.Any(b => b.BookingId == id));

        return id;
    }
}
