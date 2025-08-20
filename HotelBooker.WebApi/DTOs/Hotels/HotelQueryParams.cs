using Microsoft.AspNetCore.Mvc;

namespace HotelBooker.WebApi.DTOs.Hotels
{
    public record HotelQueryParams
    (
        [FromQuery] int PageNumber = 1,
        [FromQuery] int PageSize = 10,
        [FromQuery] string SearchTerm = ""
    );
}
