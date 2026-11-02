using _2026_10_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2026_10_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private static readonly List<Flight> Flights = new()
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

    [HttpGet]
    public ActionResult<IEnumerable<Flight>> GetAllFlights()
    {
        return Ok(Flights);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Flight> GetFlightById(int id)
    {
        var flight = Flights.FirstOrDefault(f => f.Id == id);

        if (flight is null)
        {
            return NotFound($"Flight with id {id} was not found.");
        }

        return Ok(flight);
    }

    [HttpPost("search")]
    public ActionResult<IEnumerable<Flight>> SearchFlights([FromBody] FlightSearchRequest request)
    {
        var result = Flights.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Origin))
        {
            result = result.Where(f => f.Origin.Equals(request.Origin, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Destination))
        {
            result = result.Where(f => f.Destination.Equals(request.Destination, StringComparison.OrdinalIgnoreCase));
        }

        if (request.DepartureDate.HasValue)
        {
            result = result.Where(f => f.DepartureTime.Date == request.DepartureDate.Value.Date);
        }

        return Ok(result.ToList());
    }

    [HttpPost("book")]
    public ActionResult<BookingResponse> BookFlight([FromBody] BookingRequest request)
    {
        if (request.PassengerCount <= 0)
        {
            return BadRequest("Passenger count must be at least 1.");
        }

        var flight = Flights.FirstOrDefault(f => f.Id == request.FlightId);

        if (flight is null)
        {
            return NotFound("Flight not found.");
        }

        if (flight.AvailableSeats < request.PassengerCount)
        {
            return BadRequest("There are not enough available seats on this flight.");
        }

        flight.AvailableSeats -= request.PassengerCount;

        var totalPrice = flight.Price * request.PassengerCount;

        var booking = new BookingResponse
        {
            BookingId = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            FlightNumber = flight.FlightNumber,
            PassengerName = request.PassengerName,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            PassengerCount = request.PassengerCount,
            TotalPrice = totalPrice
        };

        return Ok(booking);
    }
}
