namespace HotelBooker.Application.Models.Common
{
    public enum ErrorType
    {
        Validation,
        NotFound,
        RoomNotAvailable
    }

    public abstract class Error(ErrorType type, string message)
    {
        public ErrorType Type { get; } = type;
        public string Message { get; } = message;
    }
}
