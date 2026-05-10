using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPBD.DTOs;

public class UpdateReservationDto : IValidatableObject
{
    [Range(1, int.MaxValue), Required]
    public int? RoomId { get; set; }

    [MaxLength(100), Required]
    public string OrganizerName { get; set; } = string.Empty;

    [MaxLength(200), Required]
    public string Topic { get; set; } = string.Empty;

    [Required]
    public DateOnly? Date { get; set; }

    [Required]
    public TimeOnly? StartTime { get; set; }

    [Required]
    public TimeOnly? EndTime { get; set; }

    [RegularExpression("^(planned|confirmed|cancelled)$"), Required]
    public string Status { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime.HasValue && StartTime.HasValue && EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime must be later than StartTime.",
                [nameof(EndTime)]
            );
        }
    }
}
