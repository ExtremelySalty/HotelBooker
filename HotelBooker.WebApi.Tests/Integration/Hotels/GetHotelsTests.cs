using System.Net;
using HotelBooker.CommonTests;
using HotelBooker.WebApi.DTOs.Common;
using HotelBooker.WebApi.DTOs.Hotels;
using Shouldly;

#nullable disable
namespace HotelBooker.WebApi.Tests.Integration.Hotels
{
    [TestFixture]
    [Category(TestingCategories.Integration)]
    public class GetHotelsTests : IntegrationTestBase
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void GetHotels_OneTimeSetUp()
        {
            _httpClient = CreateClient();
        }

        [OneTimeTearDown]
        public void GetHotels_OneTimeTearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetHotels_WhenSearchingForHotelsByName_ShouldReturn200OkAndHotels()
        {
            // Arrange
            var searchTerm = "and";
            var pageNumber = 1;
            var pageSize = 10;
            var requestUri = $"/api/hotels?searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}";

            // Act
            var response = await _httpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var itemsResult = await DeserializeAsync<PaginatedItemsResultDto<HotelDto>>(response);
            itemsResult.PageNumber.ShouldBe(pageNumber);
            itemsResult.PageSize.ShouldBe(pageSize);
            itemsResult.TotalItems.ShouldBe(24);
            itemsResult.Items.Count.ShouldBe(pageSize);
        }
    }
}
#nullable enable
