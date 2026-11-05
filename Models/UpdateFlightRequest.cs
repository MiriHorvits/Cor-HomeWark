using System.ComponentModel.DataAnnotations;

namespace _2026_10_06.Models;

/// <summary>
/// עדכון חלקי של טיסה — שדות null לא משתנים.
/// </summary>
public class UpdateFlightRequest
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Available seats cannot be negative.")]
    public int? AvailableSeats { get; set; }
}
