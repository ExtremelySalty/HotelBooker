using MediatR;

namespace HotelBooker.Application.Hotels.FindByName
{
    public record FindHotelByNameRequest
    (
        string Name,
        int PageNumber,
        int PageSize
    ) : IRequest<FindHotelByNameResponse>;
}
