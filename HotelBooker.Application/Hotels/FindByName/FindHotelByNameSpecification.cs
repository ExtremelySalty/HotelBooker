using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Hotels.FindByName
{
    public class FindHotelByNameSpecification : Specification<Hotel>
    {
        public FindHotelByNameSpecification(string name)
        {
            AddFilter(x => x.Name.Contains(name));
            AddInclude(x => x.Rooms);
            AddThenInclude("Rooms.RoomType");
            SetOrderBy(x => x.Name);
        }
    }
}
