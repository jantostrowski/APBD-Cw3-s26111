using WebApplicationAPBD.DTOs;

namespace WebApplicationAPBD.Services;

public interface IReservationService
{
    IEnumerable<ReservationDto> GetAll(DateOnly? date, string? status, int? roomId);
    ReservationDto? GetById(int id);
    ReservationResult Add(CreateReservationDto reservation);
    ReservationResult Update(int id, UpdateReservationDto reservation);
    bool Remove(int id);
}
