using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using MediatR;

namespace HotelBooker.Application.Bookings.FindBookingByReference
{
    public class FindBookingByReferenceHandler
        : IRequestHandler<FindBookingByReferenceRequest, FindBookingByReferenceResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public FindBookingByReferenceHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<FindBookingByReferenceResponse> Handle
        (
            FindBookingByReferenceRequest request,
            CancellationToken cancellationToken
        )
        {
            var booking = await _bookingRepository.FindByReferenceAsync
            (
                request.Reference,
                cancellationToken
            );

            if (booking is null)
            {
                return new FindBookingByReferenceResponse(new NotFoundError("Booking not found."));
            }

            return new FindBookingByReferenceResponse(booking);
        }
    }
}
