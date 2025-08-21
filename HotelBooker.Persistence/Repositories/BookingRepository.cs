using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(ApplicationDbContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateAsync
        (
            Booking booking,
            CancellationToken ct
        )
        {
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new booking.");
                throw;
            }
        }

        public async Task<Booking?> FindByReferenceAsync
        (
            string reference,
            CancellationToken ct
        )
        {
            try
            {
                return await _context
                    .Bookings
                    .Include(x => x.Room)
                    .ThenInclude(x => x.Hotel)
                    .Include(x => x.Room)
                    .ThenInclude(x => x.RoomType)
                    .FirstOrDefaultAsync(x => x.ReferenceNumber == reference, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find booking: {ref}", reference);
                throw;
            }
        }
    }
}
