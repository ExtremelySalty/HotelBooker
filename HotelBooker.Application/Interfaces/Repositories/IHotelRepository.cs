using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface IHotelRepository : IGenericRepository<int, Hotel>
    {
    }
}
