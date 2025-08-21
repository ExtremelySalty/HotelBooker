using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Rooms.FindAvailableRooms;
using HotelBooker.CommonTests;
using HotelBooker.CommonTests.Fakers;
using HotelBooker.Domain.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Shouldly;

#nullable disable
namespace HotelBooker.Application.Tests.Rooms.FindAvailableRooms
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class FindAvailableRoomsHandlerTests
    {
        private IRoomRepository _mockRoomRepository;
        private FindAvailableRoomsHandler _handler;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockRoomRepository = Substitute.For<IRoomRepository>();

            _handler = new FindAvailableRoomsHandler
            (
                _mockRoomRepository,
                Substitute.For<ILogger<FindAvailableRoomsHandler>>()
            );
        }

        [SetUp]
        public void SetUp()
        {
            _mockRoomRepository.ClearSubstitute();
        }

        [Test]
        public async Task Handle_WhenThereIsAvailableRooms_ShouldReturnThoseRooms()
        {
            // Arrange
            var request = new FindAvailableRoomsRequest
            (
                Capacity: 2,
                StartDate: new DateTime(2025, 9, 1),
                EndDate: new DateTime(2025, 9, 15)
            );

            var room = GenerateRooms(1).First();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 8, 20),
                EndDateUtc = new DateTime(2025, 8, 25)
            });
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 10, 1),
                EndDateUtc = new DateTime(2025, 10, 20)
            });

            _mockRoomRepository.FindByCapacityAsync
            (
                Arg.Is(request.Capacity),
                Arg.Any<CancellationToken>()
            ).Returns([room]);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.AvailableRooms.Count.ShouldBe(1);
        }

        [Test]
        public async Task Handle_WhenThereIsNoAvailableRooms_ShouldReturnNoRooms()
        {
            // Arrange
            var request = new FindAvailableRoomsRequest
            (
                Capacity: 2,
                StartDate: new DateTime(2025, 9, 1),
                EndDate: new DateTime(2025, 9, 9)
            );

            var room = GenerateRooms(1).First();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 8, 20),
                EndDateUtc = new DateTime(2025, 9, 5)
            });
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 9, 10),
                EndDateUtc = new DateTime(2025, 10, 20)
            });

            _mockRoomRepository.FindByCapacityAsync
            (
                Arg.Is(request.Capacity),
                Arg.Any<CancellationToken>()
            ).Returns([room]);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.AvailableRooms.Count.ShouldBe(0);
        }

        [Test]
        public async Task Handle_WhenBookingsGoOverAYearAndIsAvailable_ShouldReturnRooms()
        {
            // Arrange
            var request = new FindAvailableRoomsRequest
            (
                Capacity: 2,
                StartDate: new DateTime(2025, 12, 1),
                EndDate: new DateTime(2026, 1, 9)
            );

            var room = GenerateRooms(1).First();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 8, 20),
                EndDateUtc = new DateTime(2025, 9, 5)
            });
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 9, 10),
                EndDateUtc = new DateTime(2025, 10, 20)
            });

            _mockRoomRepository.FindByCapacityAsync
            (
                Arg.Is(request.Capacity),
                Arg.Any<CancellationToken>()
            ).Returns([room]);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.AvailableRooms.Count.ShouldBe(1);
        }

        private static List<Room> GenerateRooms(int howMany)
        {
            var roomFaker = new RoomFaker(1297, [1, 2, 3]);
            return roomFaker.Generate(howMany);
        }
    }
}
#nullable enable
