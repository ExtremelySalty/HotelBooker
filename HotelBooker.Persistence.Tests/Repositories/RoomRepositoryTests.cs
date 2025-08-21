using HotelBooker.Application.Models.Common;
using HotelBooker.Application.Rooms.FindAvailableRooms;
using HotelBooker.CommonTests;
using HotelBooker.CommonTests.Fakers;
using HotelBooker.Domain.Models;
using HotelBooker.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Testcontainers.MsSql;

#nullable disable
namespace HotelBooker.Persistence.Tests.Repositories
{
    [TestFixture]
    [Category(TestingCategories.Repository)]
    public class RoomRepositoryTests
    {
        private MsSqlContainer _sqlContainer;
        private RoomRepository _repository;
        private HotelFaker _hotelFaker;
        private RoomFaker _roomFaker;
        private const int _seed = 1297;
        private const int _hotelsToGenerate = 10;
        private ApplicationDbContext _context;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _sqlContainer = new MsSqlBuilder().Build();
            await _sqlContainer.StartAsync();

            var dbOptions = new DbContextOptionsBuilder()
                .UseSqlServer(_sqlContainer.GetConnectionString())
                .EnableDetailedErrors()
                .Options;

            _context = new ApplicationDbContext(dbOptions);

            await _context.Database.EnsureCreatedAsync();
            await _context.AddRangeAsync(GenerateRoomTypes());
            await _context.AddRangeAsync(GenerateHotels());
            await _context.SaveChangesAsync();

            var rooms = await _context
                .Rooms
                .Select(x => x.Id)
                .ToListAsync();

            await _context.AddRangeAsync(GenerateBookings(rooms));
            await _context.SaveChangesAsync();

            _repository = new RoomRepository
            (
                _context,
                Substitute.For<ILogger<RoomRepository>>()
            );
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (_sqlContainer is not null)
            {
                await _sqlContainer.StopAsync();
                await _sqlContainer.DisposeAsync();
            }

            await _context.DisposeAsync();
        }

        [Test]
        public async Task FindByIdAsync_WhenRoomExists_ShouldReturnRoom()
        {
            // Arrange
            var roomId = 1;

            // Act
            var room = await _repository.FindByIdAsync(roomId, CancellationToken.None);

            // Assert
            room.ShouldNotBeNull();
            room.Hotel.ShouldNotBeNull();
            room.RoomType.ShouldNotBeNull();
        }

        [Test]
        public async Task FindByIdAsync_WhenRoomDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var roomId = int.MaxValue;

            // Act
            var room = await _repository.FindByIdAsync(roomId, CancellationToken.None);

            // Assert
            room.ShouldBeNull();
        }

        private static List<RoomType> GenerateRoomTypes()
            =>
            [
                new() { Name = "Single" },
                new() { Name = "Double" },
                new() { Name = "Deluxe" }
            ];

        private List<Hotel> GenerateHotels()
        {
            _roomFaker = new RoomFaker(_seed, [1, 2, 3]);
            _hotelFaker = new HotelFaker(_seed, _roomFaker);
            var hotels = _hotelFaker.Generate(_hotelsToGenerate);
            return hotels;
        }

        private static List<Booking> GenerateBookings(IEnumerable<int> roomIds)
        {
            var result = new List<Booking>();
            foreach (var roomId in roomIds)
            {
                var bookings = new List<Booking>
                {
                    new()
                    {
                        StartDateUtc = new DateTime(2025, 8, 1),
                        EndDateUtc = new DateTime(2025, 8, 12),
                        BookedOnUtc = DateTime.UtcNow,
                        RoomId = roomId,
                        ReferenceNumber = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        StartDateUtc = new DateTime(2025, 10, 25),
                        EndDateUtc = new DateTime(2025, 11, 10),
                        BookedOnUtc = DateTime.UtcNow,
                        RoomId = roomId,
                        ReferenceNumber = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        StartDateUtc = new DateTime(2025, 12, 25),
                        EndDateUtc = new DateTime(2026, 1, 12),
                        BookedOnUtc = DateTime.UtcNow,
                        RoomId = roomId,
                        ReferenceNumber = Guid.NewGuid().ToString()
                    }
                };
                result.AddRange(bookings);
            }
            return result;
        }
    }
}
#nullable enable
