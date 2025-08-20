using HotelBooker.CommonTests;
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
    public class SeedDataRepositoryTests
    {
        private MsSqlContainer _sqlContainer;
        private SeedDataRepository _repository;
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

            _repository = new SeedDataRepository
            (
                _context,
                Substitute.For<ILogger<SeedDataRepository>>()
            );
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if(_sqlContainer is not null)
            {
                await _sqlContainer.StopAsync();
                await _sqlContainer.DisposeAsync();
            }

            if (_context is not null)
            {
                await _context.DisposeAsync();
            }
        }

        [Test]
        [Order(1)]
        public async Task SeedDataAsync_WhenCalled_ShouldAddTestData()
        {
            // Arrange
            var amountToSeed = 10;

            // Act
            await _repository.SeedAsync(amountToSeed, CancellationToken.None);

            // Assert
            var hotelCount = await _context.Hotels.CountAsync();
            hotelCount.ShouldBe(amountToSeed);
        }

        [Test]
        [Order(2)]
        public async Task ResetDataAsync_WhenCalled_ShouldClearThenReaddTestData()
        {
            // Arrange & Act
            await _repository.ResetAsync(CancellationToken.None);

            // Assert
            var dataExists = await _context.Hotels.AnyAsync();
            dataExists.ShouldBeFalse();
        }
    }
}
#nullable enable
