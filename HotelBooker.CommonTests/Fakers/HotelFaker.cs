using Bogus;
using HotelBooker.Domain.Models;

namespace HotelBooker.CommonTests.Fakers
{
    public class HotelFaker : Faker<Hotel>
    {
        public HotelFaker(int seed, RoomFaker roomFaker)
        {
            UseSeed(seed)
                .RuleFor(h => h.Name, f => f.Company.CompanyName())
                .RuleFor(h => h.Code, f => f.Random.String2(5, 15))
                .RuleFor(h => h.Location, f => f.Address.City())
                .RuleFor(h => h.Rooms, f => roomFaker.Generate(6));
        }
    }
}
