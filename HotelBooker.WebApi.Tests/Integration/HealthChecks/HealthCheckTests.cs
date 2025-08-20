using HotelBooker.CommonTests;
using Shouldly;

#nullable disable
namespace HotelBooker.WebApi.Tests.Integration.HealthChecks
{
    [TestFixture]
    [Category(TestingCategories.Integration)]
    public class HealthCheckTests : IntegrationTestBase
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void HealthCheck_OneTimeSetUp()
        {
            _httpClient = CreateClient();
        }

        [OneTimeTearDown]
        public void HealthCheck_OneTimeTearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetHealthCheck_WhenAppIsHealthly_ShouldReturn200Ok()
        {
            // Arrange & Act
            var response = await _httpClient.GetAsync("/health");

            // Assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Healthy");
        }
    }
}
#nullable enable
