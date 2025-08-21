namespace HotelBooker.WebApi.DTOs.Common
{
    public record ItemsResultDto<TEntity>(IEnumerable<TEntity> Items);
}
