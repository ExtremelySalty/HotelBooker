namespace HotelBooker.Application.Models.Common
{
    public class NotFoundError(string message)
        : Error(ErrorType.NotFound, message)
    {
    }
}
