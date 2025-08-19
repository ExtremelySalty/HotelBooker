namespace HotelBooker.Domain.Models
{
    public class Booking
    {
        private Room? _room;
        private Customer? _customer;

        public int Id { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public DateTime BookedOnUtc { get; set; }
        public virtual Room Room
        {
            get => _room ?? throw new InvalidOperationException("Room is not set.");
            set => _room = value;
        }
        public virtual Customer Customer
        {
            get => _customer ?? throw new InvalidOperationException("Customer is not set.");
            set => _customer = value;
        }
    }
}
