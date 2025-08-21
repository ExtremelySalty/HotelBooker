using HotelBooker.Application.Rooms.FindAvailableRooms;
using HotelBooker.WebApi.DTOs.Common;
using HotelBooker.WebApi.DTOs.Rooms;
using HotelBooker.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResultDto<AvailableRoomDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetAvailableRoomsAsync
        (
            [FromQuery] int capacity,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken ct
        )
        {
            if (startDate >= endDate)
                return BadRequest();

            var request = new FindAvailableRoomsRequest(capacity, startDate, endDate);

            var response = await _mediator.Send(request, ct);

            var rooms = response.AvailableRooms
                .Select(x => x.ToAvailableDto());

            return Ok(new ItemsResultDto<AvailableRoomDto>(rooms));
        }
    }
}
