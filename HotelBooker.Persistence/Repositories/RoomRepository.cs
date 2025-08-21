using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Persistence.Repositories
{
    public class RoomRepository
        : GenericRepository<int, Room, RoomRepository>, IRoomRepository
    {
        public RoomRepository
        (
            ApplicationDbContext context,
            ILogger<RoomRepository> logger
        ) : base(context, logger)
        {
        }

        public async Task<IReadOnlyCollection<Room>> FindByCapacityAsync
        (
            int capacity,
            CancellationToken ct
        )
        {
            try
            {
                return await _context
                    .Rooms
                    .Include(x => x.RoomType)
                    .Include(x => x.Hotel)
                    .Include(x => x.Bookings)
                    .Where(x => x.MaxCapacity >= capacity)
                    .Select(x => new Room
                    {
                        Id = x.Id,
                        Number = x.Number,
                        Description = x.Description,
                        MaxCapacity = x.MaxCapacity,
                        HotelId = x.HotelId,
                        RoomTypeId = x.RoomTypeId,
                        Hotel = x.Hotel,
                        RoomType = x.RoomType,
                        Bookings = x.Bookings.Where(x => x.StartDateUtc >= DateTime.UtcNow).ToList()
                    })
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find by capacity.");
                throw;
            }
        }

        public override async Task<Room?> FindByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _context
                    .Rooms
                    .Include(r => r.Hotel)
                    .Include(r => r.RoomType)
                    .Include(r => r.Bookings)
                    .FirstOrDefaultAsync(r => r.Id == id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to load room: {id}.", id);
                throw;
            }
        }
    }
}
