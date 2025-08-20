using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;

namespace HotelBooker.Application.Hotels.FindByName
{
    public record FindHotelByNameResponse(QueryResponse<Hotel> Data);
}
