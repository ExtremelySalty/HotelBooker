namespace HotelBooker.WebApi.DTOs.Bookings
{
    public record BookingInfoDto
    (
        string Reference,
        DateTime StartDateUtc,
        DateTime EndDateUtc,
        DateTime BookedOnUtc,
        int HotelId,
        string Hotel
    );
}
