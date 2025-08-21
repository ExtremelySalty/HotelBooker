using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task CreateAsync(Booking booking, CancellationToken ct);
        Task<Booking?> FindByReferenceAsync(string reference, CancellationToken ct);
    }
}
