using HotelBooker.CommonTests;
using HotelBooker.WebApi.DTOs.Version;
using Shouldly;

#nullable disable
namespace HotelBooker.WebApi.Tests.Integration.Version
{
    [TestFixture]
    [Category(TestingCategories.Integration)]
    public class GetVersionTests : IntegrationTestBase
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void GetVersion_OneTimeSetUp()
        {
            _httpClient = CreateClient();
        }

        [OneTimeTearDown]
        public void GetVersion_OneTimeTearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetVersion_ShouldReturn200OkAndVersionInfo()
        {
            // Arrange & Act
            var response = await _httpClient.GetAsync("/api/version");

            // Assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var appInfo = await DeserializeAsync<VersionDto>(response);
            appInfo.ShouldNotBeNull();
            appInfo.Version.ShouldBe("1.0.0");
            appInfo.Name.ShouldBe("api-hotel-booker");
        }
    }
}
#nullable enable
