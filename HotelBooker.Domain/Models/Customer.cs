namespace HotelBooker.Domain.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public virtual List<Booking> Bookings { get; set; } = [];
    }
}
