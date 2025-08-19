using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Domain.Models;
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
                await _context.Database.EnsureDeletedAsync(ct);
                await SeedAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to reset and seed data.");
                throw;
            }
        }

        public async Task SeedAsync(CancellationToken ct)
        {
            try
            {
                var roomTypes = GenerateRoomTypes();

                await _context.Database.EnsureCreatedAsync(ct);
                await _context.AddRangeAsync(roomTypes, cancellationToken: ct);
                await _context.AddRangeAsync(GenerateHotels(roomTypes), cancellationToken: ct);
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

        private static List<Hotel> GenerateHotels(List<RoomType> roomTypes)
        {
            var hotelOne = new Hotel
            {
                Name = "Central Grand Hotel",
                Location = "Glasgow",
                Code = "CGH"
            };
            hotelOne.Rooms.AddRange
            (
                [
                    new Room
                    {
                        Number = "101",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelOne
                    },
                    new Room
                    {
                        Number = "102",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelOne
                    },
                    new Room
                    {
                        Number = "201",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelOne
                    },
                    new Room
                    {
                        Number = "202",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelOne
                    },
                    new Room
                    {
                        Number = "301",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelOne
                    },
                    new Room
                    {
                        Number = "302",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelOne
                    }
                ]
            );

            var hotelTwo = new Hotel
            {
                Name = "Malmaison",
                Location = "Dundee",
                Code = "MAL"
            };
            hotelOne.Rooms.AddRange
            (
                [
                    new Room
                    {
                        Number = "101",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelTwo
                    },
                    new Room
                    {
                        Number = "102",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelTwo
                    },
                    new Room
                    {
                        Number = "201",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelTwo
                    },
                    new Room
                    {
                        Number = "202",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelTwo
                    },
                    new Room
                    {
                        Number = "301",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelTwo
                    },
                    new Room
                    {
                        Number = "302",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelTwo
                    }
                ]
            );

            var hotelThree = new Hotel
            {
                Name = "The Balmoral",
                Location = "Edinburgh",
                Code = "BAL"
            };
            hotelOne.Rooms.AddRange
            (
                [
                    new Room
                    {
                        Number = "101",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelThree
                    },
                    new Room
                    {
                        Number = "102",
                        Description = "Single room with a view",
                        MaxCapacity = 2,
                        RoomType = roomTypes[0],
                        Hotel = hotelThree
                    },
                    new Room
                    {
                        Number = "201",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelThree
                    },
                    new Room
                    {
                        Number = "202",
                        Description = "Double room with a balcony",
                        MaxCapacity = 4,
                        RoomType = roomTypes[1],
                        Hotel = hotelThree
                    },
                    new Room
                    {
                        Number = "301",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelThree
                    },
                    new Room
                    {
                        Number = "302",
                        Description = "Deluxe room with a butler and bar service",
                        MaxCapacity = 6,
                        RoomType = roomTypes[2],
                        Hotel = hotelThree
                    }
                ]
            );

            return [hotelOne, hotelTwo, hotelThree];
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
