namespace HotelBooker.WebApi.DTOs.Common
{
    public record ItemsResultDto<TEntity>
    (
        int PageNumber,
        int PageSize,
        int TotalItems,
        IReadOnlyCollection<TEntity> Items
    );
}
