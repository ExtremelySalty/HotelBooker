namespace HotelBooker.Domain.Models
{
    public class Booking
    {
        private Room? _room;

        public int Id { get; set; }
        public int RoomId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public DateTime BookedOnUtc { get; set; }
        public virtual Room Room
        {
            get => _room ?? throw new InvalidOperationException("Room is not set.");
            set => _room = value;
        }
    }
}
