using WebApplicationAPBD.Data;
using WebApplicationAPBD.DTOs;
using WebApplicationAPBD.Models;

namespace WebApplicationAPBD.Services;

public class ReservationService : IReservationService
{
    public IEnumerable<ReservationDto> GetAll(DateOnly? date, string? status, int? roomId)
    {
        var reservations = DataStore.Reservations.AsEnumerable();

        if (date.HasValue)
        {
            reservations = reservations.Where(x => x.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(x => string.Equals(x.Status, status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(x => x.RoomId == roomId.Value);
        }

        return reservations.Select(ToDto);
    }

    public ReservationDto? GetById(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(x => x.Id == id);

        return reservation is null ? null : ToDto(reservation);
    }

    public ReservationResult Add(CreateReservationDto reservation)
    {
        var roomId = reservation.RoomId!.Value;
        var date = reservation.Date!.Value;
        var startTime = reservation.StartTime!.Value;
        var endTime = reservation.EndTime!.Value;

        if (!DataStore.Rooms.Any(x => x.Id == roomId))
        {
            return new ReservationResult { Status = ReservationResultStatus.RoomNotFound };
        }

        if (HasConflict(roomId, date, startTime, endTime, null, reservation.Status))
        {
            return new ReservationResult { Status = ReservationResultStatus.Conflict };
        }

        var reservationToAdd = new Reservation
        {
            Id = DataStore.NextReservationId++,
            RoomId = roomId,
            OrganizerName = reservation.OrganizerName,
            Topic = reservation.Topic,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Status = reservation.Status
        };

        DataStore.Reservations.Add(reservationToAdd);

        return new ReservationResult
        {
            Status = ReservationResultStatus.Success,
            Reservation = ToDto(reservationToAdd)
        };
    }

    public ReservationResult Update(int id, UpdateReservationDto reservation)
    {
        var existing = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return new ReservationResult { Status = ReservationResultStatus.ReservationNotFound };
        }

        var roomId = reservation.RoomId!.Value;
        var date = reservation.Date!.Value;
        var startTime = reservation.StartTime!.Value;
        var endTime = reservation.EndTime!.Value;

        if (!DataStore.Rooms.Any(x => x.Id == roomId))
        {
            return new ReservationResult { Status = ReservationResultStatus.RoomNotFound };
        }

        if (HasConflict(roomId, date, startTime, endTime, id, reservation.Status))
        {
            return new ReservationResult { Status = ReservationResultStatus.Conflict };
        }

        existing.RoomId = roomId;
        existing.OrganizerName = reservation.OrganizerName;
        existing.Topic = reservation.Topic;
        existing.Date = date;
        existing.StartTime = startTime;
        existing.EndTime = endTime;
        existing.Status = reservation.Status;

        return new ReservationResult
        {
            Status = ReservationResultStatus.Success,
            Reservation = ToDto(existing)
        };
    }

    public bool Remove(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(x => x.Id == id);
        if (reservation is null)
        {
            return false;
        }

        DataStore.Reservations.Remove(reservation);
        return true;
    }

    private static bool HasConflict(
        int roomId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        int? ignoredReservationId,
        string status
    )
    {
        if (string.Equals(status, "cancelled", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return DataStore.Reservations.Any(x =>
            x.RoomId == roomId &&
            x.Date == date &&
            x.Id != ignoredReservationId &&
            !string.Equals(x.Status, "cancelled", StringComparison.OrdinalIgnoreCase) &&
            startTime < x.EndTime &&
            endTime > x.StartTime
        );
    }

    private static ReservationDto ToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            OrganizerName = reservation.OrganizerName,
            Topic = reservation.Topic,
            Date = reservation.Date,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status
        };
    }
}
