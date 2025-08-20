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
                if (await _context.RoomTypes.AnyAsync(ct) == false)
                {
                    await _context.AddRangeAsync(roomTypes, cancellationToken: ct);
                    await _context.SaveChangesAsync(ct);
                }
                if (await _context.Hotels.AnyAsync(ct) == false)
                {
                    await _context.AddRangeAsync(GenerateHotels(hotelsToSeed, roomTypes.Select(x => x.Id)), cancellationToken: ct);
                }
                if(await _context.Customers.AnyAsync(ct) == false)
                {
                    await _context.AddRangeAsync(GenerateCustomers(), cancellationToken: ct);
                }
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

        private static List<Hotel> GenerateHotels(int hotelsToSeed, IEnumerable<int> roomTypes)
        {
            var roomFaker = new RoomFaker(1297, roomTypes);
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
