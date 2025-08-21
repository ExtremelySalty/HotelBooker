using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Bookings.FindBookingByReference
{
    public class FindBookingByReferenceResponse
    {
        public Booking? Booking { get; }
        public Error? Error { get; }
        public bool HasError => Error is not null;

        public FindBookingByReferenceResponse(Booking booking)
        {
            Booking = booking;
            Error = null;
        }

        public FindBookingByReferenceResponse(Error error)
        {
            Error = error;
            Booking = null;
        }
    }
}
