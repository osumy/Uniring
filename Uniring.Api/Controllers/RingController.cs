using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;

namespace Uniring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RingController : ControllerBase
    {
        private readonly IRingService _ringService;

        public RingController(IRingService ringService)
        {
            _ringService = ringService;
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> SearchRing(string uid)
        {
            var result = await _ringService.GetRingByUid(uid);

            return Ok(result);
        }
    }
}
