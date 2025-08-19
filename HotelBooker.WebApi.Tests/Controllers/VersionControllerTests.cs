using HotelBooker.CommonTests;
using HotelBooker.WebApi.Controllers;
using HotelBooker.WebApi.DTOs.Version;
using HotelBooker.WebApi.Option;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace HotelBooker.WebApi.Tests.Controllers
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class VersionControllerTests
    {
        [Test]
        public void GetVersion_ReturnsCorrectVersionDtoAndOk()
        {
            // Arrange
            var appInfo = new ApplicationInfo { Name = "HotelBooker", Version = "1.2.3" };
            var options = Substitute.For<IOptions<ApplicationInfo>>();
            options.Value.Returns(appInfo);

            var controller = new VersionController(options);

            // Act
            var result = controller.GetVersion();

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.StatusCode.ShouldBe(200);

            okResult.Value.ShouldBeOfType<VersionDto>();
            var dto = okResult.Value as VersionDto;
            dto!.Name.ShouldBe("HotelBooker");
            dto.Version.ShouldBe("1.2.3");
        }
    }
}
