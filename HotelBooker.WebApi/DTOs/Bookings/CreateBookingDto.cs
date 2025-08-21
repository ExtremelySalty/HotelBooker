namespace HotelBooker.WebApi.DTOs.Bookings
{
    public record CreateBookingDto
    (
        int RoomId,
        DateTime StartDate,
        DateTime EndDate
    );
}
