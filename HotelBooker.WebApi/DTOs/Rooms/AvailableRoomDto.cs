namespace HotelBooker.WebApi.DTOs.Rooms
{
    public record AvailableRoomDto
    (
        int RoomId,
        int HotelId,
        int RoomTypeId,
        string HotelName,
        string RoomType,
        string Number,
        int Capacity,
        string? Description
    );
}
