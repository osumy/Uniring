using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Domain.Entities;

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
        public async Task<IActionResult> GetRingByUidAsync(string uid)
        {
            var result = await _ringService.GetRingByUidAsync(uid);

            return Ok(result);
        }

        [HttpGet("serial/{serial}")]
        public async Task<IActionResult> GetRingBySerialAsync(string serial)
        {
            var result = await _ringService.GetRingBySerialAsync(serial);

            return Ok(result);
        }
    }

}
