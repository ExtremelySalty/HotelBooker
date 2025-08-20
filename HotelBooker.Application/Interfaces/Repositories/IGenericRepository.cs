using HotelBooker.Application.Models.Common;

namespace HotelBooker.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TId, TEntity>
        where TId : notnull
        where TEntity : class
    {
        Task<TEntity?> FindByIdAsync(TId id, CancellationToken ct);
        Task<QueryResponse<TEntity>> PerformQueryAsync(QueryRequest<TEntity> queryRequest, CancellationToken ct);
    }
}
