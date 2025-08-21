using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Application.Rooms.FindAvailableRooms
{
    public class FindAvailableRoomsHandler
        : IRequestHandler<FindAvailableRoomsRequest, FindAvailableRoomsResponse>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<FindAvailableRoomsHandler> _logger;

        public FindAvailableRoomsHandler
        (
            IRoomRepository roomRepository,
            ILogger<FindAvailableRoomsHandler> logger
        )
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<FindAvailableRoomsResponse> Handle
        (
            FindAvailableRoomsRequest request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var rooms = await _roomRepository.FindByCapacityAsync
                (
                    request.Capacity,
                    cancellationToken
                );

                var availableRooms = new Dictionary<int, Room>();

                foreach(var room in rooms)
                {
                    if (room.IsAvailable(request.StartDate, request.EndDate))
                    {
                        availableRooms.TryAdd(room.Id, room);
                    }
                }

                return new FindAvailableRoomsResponse(availableRooms.Values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find available rooms.");
                throw;
            }
        }
    }
}
