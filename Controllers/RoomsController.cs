using WebApplicationAPBD.DTOs;
using WebApplicationAPBD.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomService service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly
    )
    {
        return Ok(service.GetAll(minCapacity, hasProjector, activeOnly));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var room = service.GetById(id);

        return room is null
            ? NotFound(new { code = "room_not_found", message = $"Room with id {id} not found." })
            : Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuildingCode([FromRoute] string buildingCode)
    {
        return Ok(service.GetByBuildingCode(buildingCode));
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateRoomDto room)
    {
        var createdRoom = service.Add(room);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdRoom.Id },
            createdRoom
        );
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(
        [FromRoute] int id,
        [FromBody] UpdateRoomDto room
    )
    {
        var updatedRoom = service.Update(id, room);

        return updatedRoom is null
            ? NotFound(new { code = "room_not_found", message = $"Room with id {id} not found." })
            : Ok(updatedRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        return service.Remove(id) switch
        {
            RoomRemoveResult.Success => NoContent(),
            RoomRemoveResult.RoomNotFound => NotFound(new
            {
                code = "room_not_found",
                message = $"Room with id {id} not found."
            }),
            RoomRemoveResult.HasReservations => Conflict(new
            {
                code = "room_has_reservations",
                message = $"Room with id {id} cannot be deleted because it has reservations."
            }),
            _ => BadRequest()
        };
    }
}
