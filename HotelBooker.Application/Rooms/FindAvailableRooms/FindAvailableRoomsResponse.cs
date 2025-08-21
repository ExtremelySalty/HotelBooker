using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Rooms.FindAvailableRooms
{
    public class FindAvailableRoomsResponse
    {
        public IReadOnlyCollection<Room> AvailableRooms { get; }

        public FindAvailableRoomsResponse(IReadOnlyCollection<Room> rooms)
        {
            AvailableRooms = rooms;
        }
    }
}
