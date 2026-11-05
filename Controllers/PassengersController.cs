using _2026_10_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2026_10_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassengersController : ControllerBase
{
    private readonly FlightStore _store;

    public PassengersController(FlightStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Passenger>> GetAllPassengers()
    {
        return Ok(_store.Passengers);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Passenger> GetPassengerById(int id)
    {
        var passenger = _store.Passengers.FirstOrDefault(p => p.Id == id);

        if (passenger is null)
        {
            return NotFound($"Passenger with id {id} was not found.");
        }

        return Ok(passenger);
    }

    /// <summary>שליפת נוסע לפי תעודת הזהות (PassengerId) — תואם לשדה בהזמנה.</summary>
    [HttpGet("by-id-number/{passengerId}")]
    public ActionResult<Passenger> GetPassengerByPassengerId(string passengerId)
    {
        var passenger = _store.Passengers.FirstOrDefault(
            p => p.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase));

        if (passenger is null)
        {
            return NotFound($"Passenger with id number {passengerId} was not found.");
        }

        return Ok(passenger);
    }

    [HttpPost]
    public ActionResult<Passenger> CreatePassenger([FromBody] CreatePassengerRequest request)
    {
        var duplicate = _store.Passengers.Any(
            p => p.PassengerId.Equals(request.PassengerId, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            return Conflict($"A passenger with id number {request.PassengerId} already exists.");
        }

        var passenger = new Passenger
        {
            Id = _store.Passengers.Count == 0 ? 1 : _store.Passengers.Max(p => p.Id) + 1,
            FullName = request.FullName.Trim(),
            PassengerId = request.PassengerId.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            RegisteredAt = DateTime.Now
        };

        _store.Passengers.Add(passenger);

        return CreatedAtAction(
            nameof(GetPassengerById),
            new { id = passenger.Id },
            passenger);
    }
}
