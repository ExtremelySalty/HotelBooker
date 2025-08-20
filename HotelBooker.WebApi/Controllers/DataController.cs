using HotelBooker.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ISeedData _seedData;

        public DataController(ISeedData seedData)
            => _seedData = seedData;

        [HttpPost("seed")]
        public async Task<IActionResult> SeedDataAsync
        (
            CancellationToken ct,
            [FromQuery] int ammount = 10
        )
        {
            if (ammount < 1 || ammount > 60)
                return BadRequest();

            await _seedData.SeedAsync(ammount, ct);
            return Ok("Data seeded successfully.");
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetDataAsync(CancellationToken ct)
        {
            await _seedData.ResetAsync(ct);
            return Ok("Data reset successfully.");
        }
    }
}
