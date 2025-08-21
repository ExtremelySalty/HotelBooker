using HotelBooker.Application.Bookings.CreateBooking;
using HotelBooker.Domain.Models;
using HotelBooker.WebApi.DTOs.Bookings;

namespace HotelBooker.WebApi.Extensions
{
    public static class BookingExtensions
    {
        public static CreateBookingRequest ToRequest(this CreateBookingDto dto)
            => new(dto.RoomId, dto.StartDate, dto.EndDate);

        public static BookingInfoDto ToInfoDto(this Booking booking)
            => new
            (
                booking.ReferenceNumber,
                booking.StartDateUtc,
                booking.EndDateUtc,
                booking.BookedOnUtc,
                booking.Room.HotelId,
                booking.Room.Hotel.Name
            );
    }
}
