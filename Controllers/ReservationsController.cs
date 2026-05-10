using Microsoft.AspNetCore.Mvc;
using WebApplicationAPBD.DTOs;
using WebApplicationAPBD.Services;

namespace WebApplicationAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationService service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId
    )
    {
        return Ok(service.GetAll(date, status, roomId));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var reservation = service.GetById(id);

        return reservation is null
            ? NotFound(new { code = "reservation_not_found", message = $"Reservation with id {id} not found." })
            : Ok(reservation);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateReservationDto reservation)
    {
        var result = service.Add(reservation);

        return ToActionResult(result, nameof(GetById));
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(
        [FromRoute] int id,
        [FromBody] UpdateReservationDto reservation
    )
    {
        var result = service.Update(id, reservation);

        return result.Status switch
        {
            ReservationResultStatus.Success => Ok(result.Reservation),
            ReservationResultStatus.ReservationNotFound => NotFound(new
            {
                code = "reservation_not_found",
                message = $"Reservation with id {id} not found."
            }),
            ReservationResultStatus.RoomNotFound => NotFound(new
            {
                code = "room_not_found",
                message = $"Room with id {reservation.RoomId} not found."
            }),
            ReservationResultStatus.Conflict => Conflict(new
            {
                code = "reservation_time_conflict",
                message = "Reservation time conflicts with another reservation for the same room."
            }),
            _ => BadRequest()
        };
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        return service.Remove(id)
            ? NoContent()
            : NotFound(new { code = "reservation_not_found", message = $"Reservation with id {id} not found." });
    }

    private IActionResult ToActionResult(ReservationResult result, string actionName)
    {
        return result.Status switch
        {
            ReservationResultStatus.Success => CreatedAtAction(
                actionName,
                new { id = result.Reservation!.Id },
                result.Reservation
            ),
            ReservationResultStatus.RoomNotFound => NotFound(new
            {
                code = "room_not_found",
                message = "Room assigned to reservation was not found."
            }),
            ReservationResultStatus.Conflict => Conflict(new
            {
                code = "reservation_time_conflict",
                message = "Reservation time conflicts with another reservation for the same room."
            }),
            _ => BadRequest()
        };
    }
}
