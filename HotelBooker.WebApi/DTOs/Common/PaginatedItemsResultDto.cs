namespace HotelBooker.WebApi.DTOs.Common
{
    public record PaginatedItemsResultDto<TEntity>
    (
        int PageNumber,
        int PageSize,
        int TotalItems,
        IReadOnlyCollection<TEntity> Items
    );
}
