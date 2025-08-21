using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<int, Room>
    {
        Task<IReadOnlyCollection<Room>> FindByCapacityAsync(int capacity, CancellationToken ct);
    }
}
