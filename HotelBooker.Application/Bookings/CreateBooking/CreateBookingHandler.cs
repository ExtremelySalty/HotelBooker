using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Application.Bookings.CreateBooking
{
    public class CreateBookingHandler
        : IRequestHandler<CreateBookingRequest, CreateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<CreateBookingHandler> _logger;

        public CreateBookingHandler
        (
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            ILogger<CreateBookingHandler> logger
        )
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<CreateBookingResponse> Handle
        (
            CreateBookingRequest request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Validate
                var errors = Validate(request);

                if (errors.Count != 0)
                {
                    _logger.LogInformation("Request invalid.");
                    return new CreateBookingResponse(new ValidationError(errors));
                }

                var requestedRoom = await _roomRepository.FindByIdAsync(request.RoomId, cancellationToken);

                if (requestedRoom is null)
                {
                    _logger.LogInformation("Room not found: {id}", request.RoomId);
                    return new CreateBookingResponse(new NotFoundError("Room not found."));
                }

                var isAvailable = requestedRoom.IsAvailable(request.StartDate, request.EndDate);

                if (!isAvailable)
                {
                    _logger.LogInformation("Room not available: {id}", request.RoomId);
                    return new CreateBookingResponse(new RoomNotAvailableError("Room not available"));
                }

                var generatedReferenceNumber = $"{requestedRoom.Hotel.Code}-{DateTime.UtcNow:ddMMyyyy}-{GenerateRandomSuffix()}";

                var booking = new Booking
                {
                    BookedOnUtc = DateTime.UtcNow,
                    StartDateUtc = request.StartDate,
                    EndDateUtc = request.EndDate,
                    RoomId = request.RoomId,
                    ReferenceNumber = generatedReferenceNumber
                };

                await _bookingRepository.CreateAsync(booking, cancellationToken);

                return new CreateBookingResponse(generatedReferenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create booking.");
                throw;
            }
        }

        private static IDictionary<string, string[]> Validate(CreateBookingRequest request)
        {
            var errors = new Dictionary<string, string[]>();

            if (request.StartDate >= request.EndDate)
                errors.Add("EndDate", ["EndDate cannot be before StartDate"]);

            if (request.StartDate <= DateTime.UtcNow)
                errors.Add("StartDate", ["StartDate cannot be today or in the past."]);

            return errors;
        }

        private static string GenerateRandomSuffix()
        {
            var random = new Random();
            var length = 4; // this could be configurable.
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
