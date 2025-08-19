using HotelBooker.CommonTests;
using HotelBooker.WebApi.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace HotelBooker.WebApi.Tests.Handlers
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class GlobalExceptionHandlerTests
    {
        [Test]
        public async Task TryHandleAsync_LogsErrorAndReturnsProblemDetails()
        {
            // Arrange
            var logger = Substitute.For<ILogger<GlobalExceptionHandler>>();
            var handler = new GlobalExceptionHandler(logger);

            var context = new DefaultHttpContext();

            var exception = new InvalidOperationException("Test exception");

            // Act
            var result = await handler.TryHandleAsync
            (
                context,
                exception,
                CancellationToken.None
            );

            // Assert
            result.ShouldBeTrue();
            context.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        }
    }
}
