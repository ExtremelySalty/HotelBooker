using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Application.Models.Common;
using HotelBooker.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooker.Application.Hotels.FindByName
{
    public class FindHotelByNameHandler
        : IRequestHandler<FindHotelByNameRequest, FindHotelByNameResponse>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger<FindHotelByNameHandler> _logger;

        public FindHotelByNameHandler
        (
            IHotelRepository hotelRepository,
            ILogger<FindHotelByNameHandler> logger
        )
        {
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        public async Task<FindHotelByNameResponse> Handle
        (
            FindHotelByNameRequest request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var queryRequest = new QueryRequest<Hotel>
                (
                    request.PageNumber,
                    request.PageSize,
                    new FindHotelByNameSpecification(request.Name)
                );

                var response = await _hotelRepository.PerformQueryAsync
                (
                    queryRequest,
                    cancellationToken
                );

                return new FindHotelByNameResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find hotels by name: {name}.", request.Name);
                throw;
            }
        }
    }
}
