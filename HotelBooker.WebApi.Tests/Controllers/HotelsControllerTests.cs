using HotelBooker.Application.Hotels.FindByName;
using HotelBooker.Application.Models.Common;
using HotelBooker.CommonTests;
using HotelBooker.Domain.Models;
using HotelBooker.WebApi.Controllers;
using HotelBooker.WebApi.DTOs.Common;
using HotelBooker.WebApi.DTOs.Hotels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;

#nullable disable
namespace HotelBooker.WebApi.Tests.Controllers
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class HotelsControllerTests
    {
        private IMediator _mediator;
        private HotelsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new HotelsController(_mediator);
        }

        [Test]
        public async Task GetHotelsAsync_ReturnsOkWithMappedHotels()
        {
            // Arrange
            var queryParams = new HotelQueryParams(1, 2, "Test");

            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Location = "Test City",
                Code = "TST",
                Rooms = []
            };

            var pagedResult = new FindHotelByNameResponse(new QueryResponse<Hotel>(1, 2, 1, [hotel]));

            _mediator.Send(Arg.Any<FindHotelByNameRequest>(), CancellationToken.None)
                .Returns(pagedResult);

            // Act
            var result = await _controller.GetHotelsAsync(queryParams, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.ShouldBe(200);

            var itemsResult = okResult.Value.ShouldBeOfType<PaginatedItemsResultDto<HotelDto>>();
            itemsResult.PageNumber.ShouldBe(1);
            itemsResult.PageSize.ShouldBe(2);
            itemsResult.TotalItems.ShouldBe(1);
            itemsResult.Items.Count.ShouldBe(1);

            var dto = itemsResult.Items.First();
            dto.Id.ShouldBe(hotel.Id);
            dto.Name.ShouldBe(hotel.Name);
            dto.Location.ShouldBe(hotel.Location);
            dto.Code.ShouldBe(hotel.Code);
            dto.Rooms.ShouldBeEmpty();
        }
    }
}
#nullable enable
