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
