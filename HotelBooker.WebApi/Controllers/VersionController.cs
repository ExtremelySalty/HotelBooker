using HotelBooker.WebApi.DTOs.Version;
using HotelBooker.WebApi.Option;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HotelBooker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly ApplicationInfo _info;

        public VersionController(IOptions<ApplicationInfo> options)
            => _info = options.Value;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VersionDto))]
        public IActionResult GetVersion()
            => Ok(new VersionDto(_info.Name, _info.Version));
    }
}
