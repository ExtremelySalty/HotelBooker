using HotelBooker.Application.Hotels.FindByName;
using HotelBooker.WebApi.DTOs.Common;
using HotelBooker.WebApi.DTOs.Hotels;
using HotelBooker.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController
        (
            IMediator mediator,
            ILogger<HotelsController> logger
        )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResultDto<HotelDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetHotelsAsync
        (
            HotelQueryParams queryParams,
            CancellationToken ct
        )
        {
            var request = new FindHotelByNameRequest
            (
                queryParams.SearchTerm,
                queryParams.PageNumber,
                queryParams.PageSize
            );

            var result = await _mediator.Send(request, ct);

            var hotels = result.Data
                .Items
                .Select(hotel => hotel.ToDto())
                .ToList();

            var response = new ItemsResultDto<HotelDto>
            (
                result.Data.PageNumber,
                result.Data.PageSize,
                result.Data.TotalItems,
                hotels
            );

            return Ok(response);
        }
    }
}
