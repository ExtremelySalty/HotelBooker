namespace HotelBooker.Application.Models.Common
{
    public class RoomNotAvailableError(string message)
        : Error(ErrorType.RoomNotAvailable, message)
    {
    }
}
