using HotelBooker.Application.Hotels.FindByName;
using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using HotelBooker.CommonTests;
using HotelBooker.CommonTests.Fakers;
using HotelBooker.Domain.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Shouldly;

#nullable disable
namespace HotelBooker.Application.Tests.Hotels.FindByName
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class FindHotelByNameHandlerTests
    {
        private FindHotelByNameHandler _handler;
        private IHotelRepository _mockHotelRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockHotelRepository = Substitute.For<IHotelRepository>();
            _handler = new FindHotelByNameHandler
            (
                _mockHotelRepository,
                Substitute.For<ILogger<FindHotelByNameHandler>>()
            );
        }

        [SetUp]
        public void SetUp()
        {
            _mockHotelRepository.ClearSubstitute();
        }

        [Test]
        public async Task Handle_WhenRequestIsValid_ShouldReturnResults()
        {
            // Arrange
            var hotels = GenerateHotels(10);
            var request = new FindHotelByNameRequest("Search_Term", 1, 10);
            var queryResponse = new QueryResponse<Hotel>
            (
                request.PageNumber,
                request.PageSize,
                hotels.Count,
                hotels
            );

            _mockHotelRepository.PerformQueryAsync
            (
                Arg.Any<QueryRequest<Hotel>>(),
                Arg.Any<CancellationToken>()
            ).Returns(queryResponse);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Data.ShouldBeEquivalentTo(queryResponse);
        }

        private static List<Hotel> GenerateHotels(int howMany)
        {
            var roomFaker = new RoomFaker(1291, [1, 2, 3]);
            var hotelFaker = new HotelFaker(1297, roomFaker);
            return hotelFaker.Generate(howMany);
        }
    }
}
#nullable enable
