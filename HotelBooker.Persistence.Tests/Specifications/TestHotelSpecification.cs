using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;

namespace HotelBooker.Persistence.Tests.Specifications
{
    public class TestHotelSpecification : Specification<Hotel>
    {
        public TestHotelSpecification(string name)
        {
            AddFilter(x => x.Name.Contains(name));
            AddInclude(x => x.Rooms);
        }
    }
}
