using HotelBooker.Domain.Models;
using HotelBooker.WebApi.DTOs.Hotels;

namespace HotelBooker.WebApi.Extensions
{
    public static class HotelExtensions
    {
        public static HotelDto ToDto(this Hotel hotel)
            => new
            (
                hotel.Id,
                hotel.Name,
                hotel.Location,
                hotel.Code,
                [.. hotel.Rooms.Select(x => x.ToDto())]
            );

        public static RoomDto ToDto(this Room room)
            => new
            (
                room.Id,
                room.Number,
                room.Description,
                room.RoomType.Name,
                room.MaxCapacity
            );
    }
}
