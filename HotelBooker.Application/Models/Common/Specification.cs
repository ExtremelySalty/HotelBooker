using System.Linq.Expressions;

namespace HotelBooker.Application.Models.Common
{
    public abstract class Specification<TEntity>
    {
        private Expression<Func<TEntity, bool>>? _filter;
        private Expression<Func<TEntity, object>>? _orderBy;
        private Expression<Func<TEntity, object>>? _orderByDescending;
        private List<Expression<Func<TEntity, object>>> _includes = [];

        public Expression<Func<TEntity, bool>>? Filter => _filter;
        public Expression<Func<TEntity, object>>? OrderBy => _orderBy;
        public Expression<Func<TEntity, object>>? OrderByDescending => _orderByDescending;
        public IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes
            => _includes.AsReadOnly();

        protected void AddFilter(Expression<Func<TEntity, bool>> filter)
            => _filter = filter;

        protected void SetOrderBy(Expression<Func<TEntity, object>> orderBy)
            => _orderBy = orderBy;

        protected void SetOrderByDescending(Expression<Func<TEntity, object>> orderByDesc)
            => _orderByDescending = orderByDesc;

        protected void AddInclude(Expression<Func<TEntity, object>> include)
            => _includes.Add(include);
    }
}
