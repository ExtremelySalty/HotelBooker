using Bogus;
using HotelBooker.Domain.Models;

namespace HotelBooker.CommonTests.Fakers
{
    public class RoomFaker : Faker<Room>
    {
        public RoomFaker
        (
            int seed,
            IEnumerable<int> roomTypeIds,
            int hotelId
        )
        {
            UseSeed(seed)
                .RuleFor(r => r.Number, f => f.Random.Number(600).ToString())
                .RuleFor(r => r.RoomTypeId, f => f.PickRandom(roomTypeIds))
                .RuleFor(r => r.HotelId, f => hotelId)
                .RuleFor(r => r.Description, f => f.Lorem.Sentence().OrNull(f))
                .RuleFor(r => r.MaxCapacity, f => f.Random.Number(1, 6));
        }
    }
}
