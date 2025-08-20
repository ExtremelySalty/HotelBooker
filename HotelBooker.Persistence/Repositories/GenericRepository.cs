using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Persistence.Repositories
{
    public abstract class GenericRepository<TId, TEntity, TRepository>
        : IGenericRepository<TId, TEntity>
        where TId : notnull
        where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger<TRepository> _logger;

        protected GenericRepository
        (
            ApplicationDbContext context,
            ILogger<TRepository> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public abstract Task<TEntity?> FindByIdAsync(TId id, CancellationToken ct);

        public async Task<QueryResponse<TEntity>> PerformQueryAsync
        (
            QueryRequest<TEntity> request,
            CancellationToken ct
        )
        {
            try
            {
                var queryable = _context.Set<TEntity>().AsNoTracking();

                if (request.Specification.Filter is not null)
                    queryable = queryable.Where(request.Specification.Filter);

                queryable = request.Specification
                    .Includes
                    .Aggregate(queryable, (current, include) => current.Include(include));

                var totalItems = await queryable.CountAsync(ct);

                if (request.Specification.OrderBy is not null)
                    queryable = queryable.OrderBy(request.Specification.OrderBy);
                else if (request.Specification.OrderByDescending is not null)
                    queryable = queryable.OrderByDescending(request.Specification.OrderByDescending);

                var items = await queryable
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(ct);

                return new QueryResponse<TEntity>
                (
                    request.PageNumber,
                    request.PageSize,
                    totalItems,
                    items
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to perform query for: {entity}.", typeof(TEntity).Name);
                throw;
            }
        }
    }
}
