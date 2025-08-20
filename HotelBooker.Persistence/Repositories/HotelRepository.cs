using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Persistence.Repositories
{
    public class HotelRepository
        : GenericRepository<int, Hotel, HotelRepository>, IHotelRepository
    {
        public HotelRepository
        (
            ApplicationDbContext context,
            ILogger<HotelRepository> logger
        ) : base(context, logger)
        {
        }

        public override async Task<Hotel?> FindByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _context
                    .Hotels
                    .Include(x => x.Rooms)
                    .ThenInclude(x => x.RoomType)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to load hotel: {id}.", id);
                throw;
            }
        }
    }
}
