using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;

namespace Uniring.Api.Controllers
{
    
    public class RingController : ApiControllerBase
    {
        private readonly IRingService _ringService;

        public RingController(IRingService ringService)
        {
            _ringService = ringService;
        }

        [HttpGet("uid/{uid}")]
        public async Task<IActionResult> GetRingByUid(string uid)
        {
            var result = await _ringService.GetRingByUidAsync(uid);

            return Ok(result);
        }

        [HttpGet("serial/{serial}")]
        public async Task<IActionResult> GetRingBySerial(string serial)
        {
            var result = await _ringService.GetRingBySerialAsync(serial);

            return Ok(result);
        }
    }

}
