using _2026_10_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2026_10_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly FlightStore _store;

    public FlightsController(FlightStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Flight>> GetAllFlights()
    {
        return Ok(_store.Flights);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Flight> GetFlightById(int id)
    {
        var flight = _store.FindFlight(id);

        if (flight is null)
        {
            return NotFound($"Flight with id {id} was not found.");
        }

        return Ok(flight);
    }

    [HttpGet("{id:int}/seats")]
    public ActionResult<object> GetSeatAvailability(int id)
    {
        var flight = _store.FindFlight(id);

        if (flight is null)
        {
            return NotFound($"Flight with id {id} was not found.");
        }

        return Ok(new
        {
            flight.Id,
            flight.FlightNumber,
            flight.AvailableSeats
        });
    }

    [HttpGet("stats")]
    public ActionResult<FlightStats> GetStats()
    {
        var flights = _store.Flights;

        if (flights.Count == 0)
        {
            return Ok(new FlightStats());
        }

        var stats = new FlightStats
        {
            TotalFlights = flights.Count,
            TotalAvailableSeats = flights.Sum(f => f.AvailableSeats),
            AveragePrice = Math.Round(flights.Average(f => f.Price), 2),
            PotentialRevenue = flights.Sum(f => f.Price * f.AvailableSeats),
            ByDestination = flights
                .GroupBy(f => f.Destination)
                .Select(g => new DestinationStats
                {
                    Destination = g.Key,
                    Flights = g.Count(),
                    CheapestPrice = g.Min(f => f.Price)
                })
                .OrderBy(d => d.Destination)
                .ToList()
        };

        return Ok(stats);
    }

    [HttpPost("search")]
    public ActionResult<IEnumerable<Flight>> SearchFlights([FromBody] FlightSearchRequest request)
    {
        if (request.Origin is not null
            && request.Destination is not null
            && request.Origin.Trim().Equals(request.Destination.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Origin and destination must be different.");
        }

        var result = _store.Flights.AsEnumerable();

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

        if (string.IsNullOrWhiteSpace(request.PassengerName))
        {
            return BadRequest("Passenger name is required.");
        }

        var flight = _store.FindFlight(request.FlightId);

        if (flight is null)
        {
            return NotFound("Flight not found.");
        }

        if (flight.AvailableSeats < request.PassengerCount)
        {
            return BadRequest("There are not enough available seats on this flight.");
        }

        flight.AvailableSeats -= request.PassengerCount;

        var booking = new Booking
        {
            BookingId = _store.NewBookingId(),
            FlightId = flight.Id,
            FlightNumber = flight.FlightNumber,
            PassengerName = request.PassengerName,
            PassengerId = request.PassengerId,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            PassengerCount = request.PassengerCount,
            TotalPrice = flight.Price * request.PassengerCount
        };

        _store.Bookings.Add(booking);

        return Ok(booking.ToResponse());
    }

    [HttpPatch("{id:int}")]
    public ActionResult<Flight> UpdateFlight(int id, [FromBody] UpdateFlightRequest request)
    {
        var flight = _store.FindFlight(id);

        if (flight is null)
        {
            return NotFound($"Flight with id {id} was not found.");
        }

        if (request.Price.HasValue)
        {
            flight.Price = request.Price.Value;
        }

        if (request.AvailableSeats.HasValue)
        {
            flight.AvailableSeats = request.AvailableSeats.Value;
        }

        return Ok(flight);
    }
}
