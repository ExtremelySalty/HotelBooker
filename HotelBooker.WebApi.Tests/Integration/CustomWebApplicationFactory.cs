using HotelBooker.CommonTests.Fakers;
using HotelBooker.Domain.Models;
using HotelBooker.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

#nullable disable
namespace HotelBooker.WebApi.Tests.Integration
{
    public abstract class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        private MsSqlContainer _sqlContainer;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _sqlContainer = new MsSqlBuilder()
                .Build();
            await _sqlContainer.StartAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if(_sqlContainer is not null)
            {
                await _sqlContainer.StopAsync();
                await _sqlContainer.DisposeAsync();
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                );

                if (dbContextDescriptor is not null)
                    services.Remove(dbContextDescriptor);

                services.AddDbContext<ApplicationDbContext>(config => {
                    config.UseSqlServer(_sqlContainer.GetConnectionString());
                });

                using var scope = services
                    .BuildServiceProvider()
                    .CreateScope();

                var dbContext = scope
                    .ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                dbContext.Database.EnsureCreated();
                dbContext.AddRange(GenerateRoomTypes());
                dbContext.AddRange(GenerateHotels());
                dbContext.SaveChanges();
            });
        }

        private static List<RoomType> GenerateRoomTypes()
            =>
            [
                new() { Name = "Single" },
                new() { Name = "Double" },
                new() { Name = "Deluxe" }
            ];

        private static List<Hotel> GenerateHotels()
        {
            var roomFaker = new RoomFaker(1297, [1, 2, 3]);
            var hotelFaker = new HotelFaker(1207, roomFaker);
            return hotelFaker.Generate(50);
        }
    }
}
#nullable enable
