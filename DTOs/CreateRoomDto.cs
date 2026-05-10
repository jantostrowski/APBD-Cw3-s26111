using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPBD.DTOs;

public class CreateRoomDto
{
    [MaxLength(100), Required]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20), Required]
    public string BuildingCode { get; set; } = string.Empty;

    [Required]
    public int? Floor { get; set; }

    [Range(1, 300), Required]
    public int? Capacity { get; set; }

    [Required]
    public bool? HasProjector { get; set; }

    [Required]
    public bool? IsActive { get; set; }
}
