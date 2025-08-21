using HotelBooker.Application.Models.Common;

namespace HotelBooker.Application.Bookings.CreateBooking
{
    public class CreateBookingResponse
    {
        public string? Reference { get; }
        public Error? Error { get; }
        public bool HasError => Error is not null;

        public CreateBookingResponse(string reference)
        {
            Reference = reference;
            Error = null;
        }

        public CreateBookingResponse(Error error)
        {
            Error = error;
            Reference = null;
        }
    }
}
