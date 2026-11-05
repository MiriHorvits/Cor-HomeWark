namespace _2026_10_06.Models;

public class FlightStats
{
    public int TotalFlights { get; set; }
    public int TotalAvailableSeats { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal PotentialRevenue { get; set; }
    public IReadOnlyList<DestinationStats> ByDestination { get; set; } = Array.Empty<DestinationStats>();
}

public class DestinationStats
{
    public string Destination { get; set; } = string.Empty;
    public int Flights { get; set; }
    public decimal CheapestPrice { get; set; }
}
