namespace HotelBooker.Domain.Models
{
    public class Room
    {
        private RoomType? _roomType;
        private Hotel? _hotel;

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public virtual RoomType RoomType
        {
            get => _roomType ?? throw new InvalidOperationException("RoomType is not set.");
            set => _roomType = value;
        }
        public virtual Hotel Hotel
        {
            get => _hotel ?? throw new InvalidOperationException("Hotel is not set.");
            set => _hotel = value;
        }
        public virtual List<Booking> Bookings { get; set; } = [];
    }
}
