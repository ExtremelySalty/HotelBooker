namespace HotelBooker.Domain.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public virtual List<Room> Rooms { get; set; } = [];
    }
}
