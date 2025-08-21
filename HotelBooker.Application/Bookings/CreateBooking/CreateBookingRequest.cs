using MediatR;

namespace HotelBooker.Application.Bookings.CreateBooking
{
    public record CreateBookingRequest
    (
        int RoomId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<CreateBookingResponse>;
}
