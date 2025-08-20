using HotelBooker.CommonTests;
using HotelBooker.Domain.Models;
using HotelBooker.WebApi.Extensions;
using Shouldly;

namespace HotelBooker.WebApi.Tests.Extensions
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class HotelExtensionsTests
    {
        [Test]
        public void ToDto_MapsRoomToRoomDtoCorrectly()
        {
            // Arrange
            var roomType = new RoomType { Id = 1, Name = "Deluxe" };
            var room = new Room
            {
                Id = 10,
                Number = "101",
                Description = "A nice room",
                MaxCapacity = 4,
                RoomType = roomType
            };

            // Act
            var dto = room.ToDto();

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(10);
            dto.Number.ShouldBe("101");
            dto.Description.ShouldBe("A nice room");
            dto.RoomType.ShouldBe("Deluxe");
            dto.MaxCapacity.ShouldBe(4);
        }

        [Test]
        public void ToDto_MapsHotelToHotelDtoWithRooms()
        {
            // Arrange
            var roomType = new RoomType { Id = 2, Name = "Single" };
            var room1 = new Room
            {
                Id = 1,
                Number = "201",
                Description = "Single room",
                MaxCapacity = 1,
                RoomType = roomType
            };
            var room2 = new Room
            {
                Id = 2,
                Number = "202",
                Description = "Another single room",
                MaxCapacity = 1,
                RoomType = roomType
            };
            var hotel = new Hotel
            {
                Id = 5,
                Name = "Test Hotel",
                Location = "Test City",
                Code = "TST",
                Rooms = new List<Room> { room1, room2 }
            };

            // Act
            var dto = hotel.ToDto();

            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(5);
            dto.Name.ShouldBe("Test Hotel");
            dto.Location.ShouldBe("Test City");
            dto.Code.ShouldBe("TST");
            dto.Rooms.Count.ShouldBe(2);

            dto.Rooms.ShouldContain(r => r.Id == 1 && r.Number == "201" && r.RoomType == "Single");
            dto.Rooms.ShouldContain(r => r.Id == 2 && r.Number == "202" && r.RoomType == "Single");
        }
    }
}
