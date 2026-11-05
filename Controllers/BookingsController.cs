using _2026_10_06.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2026_10_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly FlightStore _store;

    public BookingsController(FlightStore store)
    {
        _store = store;
    }

    /// <summary>כל ההזמנות הפעילות. לביטולים יש endpoint נפרד.</summary>
    [HttpGet]
    public ActionResult<IEnumerable<BookingResponse>> GetAllBookings([FromQuery] bool includeCancelled = false)
    {
        var bookings = includeCancelled
            ? _store.Bookings
            : _store.Bookings.Where(b => !b.IsCancelled);

        return Ok(bookings.Select(b => b.ToResponse()).ToList());
    }

    [HttpGet("{bookingId}")]
    public ActionResult<BookingResponse> GetBooking(string bookingId)
    {
        var booking = _store.Bookings.FirstOrDefault(
            b => b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase));

        if (booking is null)
        {
            return NotFound($"Booking {bookingId} was not found.");
        }

        return Ok(booking.ToResponse());
    }

    /// <summary>ביטול הזמנה — מחזיר את המקומות לטיסה. ביטול כפול מוחזר כ-409.</summary>
    [HttpDelete("{bookingId}")]
    public ActionResult<BookingResponse> CancelBooking(string bookingId)
    {
        var booking = _store.Bookings.FirstOrDefault(
            b => b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase));

        if (booking is null)
        {
            return NotFound($"Booking {bookingId} was not found.");
        }

        if (booking.IsCancelled)
        {
            return Conflict($"Booking {bookingId} was already cancelled.");
        }

        var flight = _store.FindFlight(booking.FlightId);

        if (flight is not null)
        {
            flight.AvailableSeats += booking.PassengerCount;
        }

        booking.IsCancelled = true;
        booking.CancelledAt = DateTime.Now;

        return Ok(booking.ToResponse());
    }
}
