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
                    await _context.AddRangeAsync
                    (
                        GenerateHotels
                        (
                            hotelsToSeed,
                            roomTypes.Select(x => x.Id)
                        ), cancellationToken: ct);
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

        private static List<Hotel> GenerateHotels
        (
            int hotelsToSeed,
            IEnumerable<int> roomTypes
        )
        {
            // for the hotels in each of them do a room with the following bookings
            // 10 days within the same month: 2nd to the 12th
            // 10 days overlapping months: 25th to the 5th
            // 10 days overlapping years: 31st/year to the 9th/year+1
            var roomFaker = new RoomFaker(1297, roomTypes);
            var hotelFaker = new HotelFaker(1297, roomFaker);
            var hotels = hotelFaker.Generate(hotelsToSeed);
            var rng = new Random();
            var bookings = new List<Booking>
            {
                new()
                {
                    StartDateUtc = new DateTime(2025, 8, 20),
                    EndDateUtc = new DateTime(2025, 8, 30),
                    ReferenceNumber = Guid.NewGuid().ToString(),
                    BookedOnUtc = DateTime.UtcNow
                },
                new()
                {
                    StartDateUtc = new DateTime(2025, 10, 25),
                    EndDateUtc = new DateTime(2025, 11, 4),
                    ReferenceNumber = Guid.NewGuid().ToString(),
                    BookedOnUtc = DateTime.UtcNow
                },
                new()
                {
                    StartDateUtc = new DateTime(2025, 12, 25),
                    EndDateUtc = new DateTime(2026, 1, 4),
                    ReferenceNumber = Guid.NewGuid().ToString(),
                    BookedOnUtc = DateTime.UtcNow
                },
            };

            foreach (var hotel in hotels)
            {
                hotel.Rooms.First().Bookings = bookings;
            }
            return hotels;
        }
    }
}
