using WebApplicationAPBD.Data;
using WebApplicationAPBD.DTOs;
using WebApplicationAPBD.Models;

namespace WebApplicationAPBD.Services;

public class RoomService : IRoomService
{
    public IEnumerable<RoomDto> GetAll(int? minCapacity, bool? hasProjector, bool? activeOnly)
    {
        var rooms = DataStore.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(x => x.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(x => x.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            rooms = rooms.Where(x => x.IsActive);
        }

        return rooms.Select(ToDto);
    }

    public IEnumerable<RoomDto> GetByBuildingCode(string buildingCode)
    {
        return DataStore.Rooms
            .Where(x => string.Equals(x.BuildingCode, buildingCode, StringComparison.OrdinalIgnoreCase))
            .Select(ToDto);
    }

    public RoomDto? GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);

        return room is null ? null : ToDto(room);
    }

    public RoomDto Add(CreateRoomDto room)
    {
        var roomToAdd = new Room
        {
            Id = DataStore.NextRoomId++,
            Name = room.Name,
            BuildingCode = room.BuildingCode,
            Floor = room.Floor!.Value,
            Capacity = room.Capacity!.Value,
            HasProjector = room.HasProjector!.Value,
            IsActive = room.IsActive!.Value
        };

        DataStore.Rooms.Add(roomToAdd);

        return ToDto(roomToAdd);
    }

    public RoomDto? Update(int id, UpdateRoomDto room)
    {
        var existing = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return null;
        }

        existing.Name = room.Name;
        existing.BuildingCode = room.BuildingCode;
        existing.Floor = room.Floor!.Value;
        existing.Capacity = room.Capacity!.Value;
        existing.HasProjector = room.HasProjector!.Value;
        existing.IsActive = room.IsActive!.Value;

        return ToDto(existing);
    }

    public RoomRemoveResult Remove(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(x => x.Id == id);
        if (room is null)
        {
            return RoomRemoveResult.RoomNotFound;
        }

        if (DataStore.Reservations.Any(x => x.RoomId == id))
        {
            return RoomRemoveResult.HasReservations;
        }

        DataStore.Rooms.Remove(room);
        return RoomRemoveResult.Success;
    }

    private static RoomDto ToDto(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            BuildingCode = room.BuildingCode,
            Floor = room.Floor,
            Capacity = room.Capacity,
            HasProjector = room.HasProjector,
            IsActive = room.IsActive
        };
    }
}
