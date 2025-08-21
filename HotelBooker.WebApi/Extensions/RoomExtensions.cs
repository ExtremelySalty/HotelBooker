using HotelBooker.Domain.Models;
using HotelBooker.WebApi.DTOs.Rooms;

namespace HotelBooker.WebApi.Extensions
{
    public static class RoomExtensions
    {
        public static AvailableRoomDto ToAvailableDto(this Room room)
            => new
            (
                room.Id,
                room.HotelId,
                room.RoomTypeId,
                room.Hotel.Name,
                room.RoomType.Name,
                room.Number,
                room.MaxCapacity,
                room.Description
            );
    }
}
