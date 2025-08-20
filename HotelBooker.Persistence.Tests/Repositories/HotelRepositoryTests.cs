using HotelBooker.Application.Models.Common;
using HotelBooker.CommonTests;
using HotelBooker.CommonTests.Fakers;
using HotelBooker.Domain.Models;
using HotelBooker.Persistence.Repositories;
using HotelBooker.Persistence.Tests.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Org.BouncyCastle.Crypto.Prng;
using Shouldly;
using Testcontainers.MsSql;

#nullable disable
namespace HotelBooker.Persistence.Tests.Repositories
{
    [TestFixture]
    [Category(TestingCategories.Repository)]
    public class HotelRepositoryTests
    {
        private MsSqlContainer _sqlContainer;
        private HotelRepository _repository;
        private HotelFaker _hotelFaker;
        private RoomFaker _roomFaker;
        private const int _seed = 1297;
        private const int _hotelsToGenerate = 10;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _sqlContainer = new MsSqlBuilder().Build();
            await _sqlContainer.StartAsync();

            var dbOptions = new DbContextOptionsBuilder()
                .UseSqlServer(_sqlContainer.GetConnectionString())
                .EnableDetailedErrors()
                .Options;

            var context = new ApplicationDbContext(dbOptions);

            await context.Database.EnsureCreatedAsync();
            await context.AddRangeAsync(GenerateRoomTypes());
            await context.AddRangeAsync(GenerateHotels());
            await context.SaveChangesAsync();

            _repository = new HotelRepository
            (
                context,
                Substitute.For<ILogger<HotelRepository>>()
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
        }

        [Test]
        public async Task FindByIdAsync_WhenHotelExists_ShouldReturnHotel()
        {
            // Arrange
            var hotelId = 1;

            // Act
            var hotel = await _repository.FindByIdAsync(hotelId, CancellationToken.None);

            // Assert
            hotel.ShouldNotBeNull();
            hotel.Rooms.Count.ShouldBe(6);
        }

        [Test]
        public async Task FindByIdAsync_WhenHotelDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var hotelId = _hotelsToGenerate + 5;

            // Act
            var hotel = await _repository.FindByIdAsync(hotelId, CancellationToken.None);

            // Assert
            hotel.ShouldBeNull();;
        }

        [Test]
        public async Task PerformQueryAsync_WhenSpecificationIsSet_ShouldReturnHotels()
        {
            // Arrange
            var specification = new TestHotelSpecification("and");
            var request = new QueryRequest<Hotel>(1, 50, specification);

            // Act
            var result = await _repository.PerformQueryAsync(request, CancellationToken.None);

            // Assert
            result.PageNumber.ShouldBe(request.PageNumber);
            result.PageSize.ShouldBe(request.PageSize);
            result.TotalItems.ShouldBe(3);
            result.Items.Count.ShouldBe(3);
            result.Items.ShouldAllBe(x => x.Rooms.Any());
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
            _roomFaker = new RoomFaker(_seed, [1,2,3]);
            _hotelFaker = new HotelFaker(_seed, _roomFaker);
            return _hotelFaker.Generate(_hotelsToGenerate);
        }
    }
}
#nullable enable
