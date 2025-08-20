namespace HotelBooker.Application.Models.Common
{
    public record QueryResponse<TEntity>
    (
        int PageNumber,
        int PageSize,
        int TotalItems,
        IReadOnlyCollection<TEntity> Items
    );
}
