using WebApplicationAPBD.DTOs;

namespace WebApplicationAPBD.Services;

public class ReservationResult
{
    public ReservationResultStatus Status { get; set; }
    public ReservationDto? Reservation { get; set; }
}

public enum ReservationResultStatus
{
    Success,
    ReservationNotFound,
    RoomNotFound,
    Conflict
}
