using WebApplicationAPBD.Models;

namespace WebApplicationAPBD.Data;

public static class DataStore
{
    public static int NextRoomId { get; set; } = 5;
    public static int NextReservationId { get; set; } = 5;

    public static List<Room> Rooms { get; } =
    [
        new Room
        {
            Id = 1,
            Name = "Sunny Room",
            BuildingCode = "A",
            Floor = 1,
            Capacity = 24,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 2,
            Name = "Cloudy Room",
            BuildingCode = "A",
            Floor = 2,
            Capacity = 16,
            HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 3,
            Name = "Rainbow Room",
            BuildingCode = "B",
            Floor = 0,
            Capacity = 40,
            HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 4,
            Name = "Workshop Lab",
            BuildingCode = "C",
            Floor = 3,
            Capacity = 20,
            HasProjector = true,
            IsActive = false
        }
    ];

    public static List<Reservation> Reservations { get; } =
    [
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Anna Kowalska",
            Topic = "Prezentacja od zera do developera",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(11, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Jan Nowak",
            Topic = "Warsztaty sieciowe",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(14, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Marta Bednarz",
            Topic = "Project management vs Product ownership",
            Date = new DateOnly(2026, 5, 11),
            StartTime = new TimeOnly(10, 30),
            EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4,
            RoomId = 4,
            OrganizerName = "Piotr Gawryluk",
            Topic = "Warsztaty odpowiedzialne planowanie",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(10, 0),
            Status = "cancelled"
        }
    ];
}
