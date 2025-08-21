using MediatR;

namespace HotelBooker.Application.Rooms.FindAvailableRooms
{
    public record FindAvailableRoomsRequest
    (
        int Capacity,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<FindAvailableRoomsResponse>;
}
