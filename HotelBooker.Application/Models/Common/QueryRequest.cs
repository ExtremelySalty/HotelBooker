namespace HotelBooker.Application.Models.Common
{
    public record QueryRequest<TEntity> 
    (
        int PageNumber,
        int PageSize,
        Specification<TEntity> Specification
    );
}
