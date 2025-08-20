using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Domain.Models;
using HotelBooker.Persistence.DataSeeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Persistence.Repositories
{
    public class SeedDataRepository : ISeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeedDataRepository> _logger;

        public SeedDataRepository
        (
            ApplicationDbContext context,
            ILogger<SeedDataRepository> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public async Task ResetAsync(CancellationToken ct)
        {
            try
            {
                if (await _context.Database.CanConnectAsync(ct))
                {
                    await _context.Bookings.ExecuteDeleteAsync(ct);
                    await _context.Rooms.ExecuteDeleteAsync(ct);
                    await _context.RoomTypes.ExecuteDeleteAsync(ct);
                    await _context.Customers.ExecuteDeleteAsync(ct);
                    await _context.Hotels.ExecuteDeleteAsync(ct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to reset and seed data.");
                throw;
            }
        }

        public async Task SeedAsync(int hotelsToSeed, CancellationToken ct)
        {
            try
            {
                var roomTypes = GenerateRoomTypes();

                await _context.Database.EnsureCreatedAsync(ct);
                await _context.AddRangeAsync(roomTypes, cancellationToken: ct);
                await _context.AddRangeAsync(GenerateHotels(hotelsToSeed), cancellationToken: ct);
                await _context.AddRangeAsync(GenerateCustomers(), cancellationToken: ct);
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to seed data.");
                throw;
            }
        }

        private static List<RoomType> GenerateRoomTypes()
            =>
            [
                new RoomType() { Name = "Single" },
                new RoomType() { Name = "Double" },
                new RoomType() { Name = "Deluxe" }
            ];

        private static List<Hotel> GenerateHotels(int hotelsToSeed)
        {
            var roomFaker = new RoomFaker(1297, [1,2,3]);
            var hotelFaker = new HotelFaker(1297, roomFaker);
            return hotelFaker.Generate(hotelsToSeed);
        }

        private static List<Customer> GenerateCustomers()
            =>
            [
                new() { Name = "David Jason", Email = "david@touchfrost.com" },
                new() { Name = "Alan Davies", Email = "jonathon@creek.com" },
                new() { Name = "Sherlock Holmes", Email = "Sherlock@hounds.com" }
            ];
    }
}
