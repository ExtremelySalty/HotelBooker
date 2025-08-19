namespace HotelBooker.Domain.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<Room> Rooms { get; set; } = [];
    }
}
