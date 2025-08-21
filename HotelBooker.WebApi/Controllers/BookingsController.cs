using HotelBooker.Application.Bookings.FindBookingByReference;
using HotelBooker.Application.Models.Common;
using HotelBooker.WebApi.DTOs.Bookings;
using HotelBooker.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateBookingAsync
        (
            [FromBody] CreateBookingDto dto,
            CancellationToken ct
        )
        {
            var response = await _mediator.Send(dto.ToRequest(), ct);

            if (response.HasError)
            {
                return HandleError(response.Error!);
            }

            return Created($"/api/booking/{response.Reference}", new { response.Reference });
        }

        [HttpGet]
        [Route("{reference}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingInfoDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetByReferenceAsync
        (
            [FromRoute] string reference,
            CancellationToken ct
        )
        {
            var response = await _mediator.Send(new FindBookingByReferenceRequest(reference), ct);
            
            if (response.HasError)
            {
                return HandleError(response.Error!);
            }

            return Ok(response.Booking!.ToInfoDto());
        }

        private ObjectResult HandleError(Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => Problem(statusCode: StatusCodes.Status404NotFound),
                ErrorType.RoomNotAvailable => Problem(statusCode: StatusCodes.Status409Conflict),
                ErrorType.Validation => HandleValidation((ValidationError)error),
                _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        }

        private static BadRequestObjectResult HandleValidation(ValidationError error)
        {
            return new BadRequestObjectResult
            (
                new ValidationProblemDetails(error.Errors)
            );
        }
    }
}
