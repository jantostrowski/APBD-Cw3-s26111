using WebApplicationAPBD.DTOs;

namespace WebApplicationAPBD.Services;

public interface IRoomService
{
    IEnumerable<RoomDto> GetAll(int? minCapacity, bool? hasProjector, bool? activeOnly);
    IEnumerable<RoomDto> GetByBuildingCode(string buildingCode);
    RoomDto? GetById(int id);
    RoomDto Add(CreateRoomDto room);
    RoomDto? Update(int id, UpdateRoomDto room);
    RoomRemoveResult Remove(int id);
}
