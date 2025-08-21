namespace HotelBooker.WebApi.DTOs.Hotels
{
    public record HotelDto
    (
        int Id,
        string Name,
        string Location,
        string Code,
        IReadOnlyCollection<HotelRoomDto> Rooms
    );

    public record HotelRoomDto
    (
        int Id,
        string Number,
        string? Description,
        string RoomType,
        int MaxCapacity
    );
}
