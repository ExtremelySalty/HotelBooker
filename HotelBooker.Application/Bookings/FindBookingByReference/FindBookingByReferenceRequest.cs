using MediatR;

namespace HotelBooker.Application.Bookings.FindBookingByReference
{
    public record FindBookingByReferenceRequest(string Reference)
        : IRequest<FindBookingByReferenceResponse>;
}
