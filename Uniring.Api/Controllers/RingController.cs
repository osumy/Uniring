using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Ring;

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
        public async Task<ActionResult<RingResponse?>> GetRingByUid([FromRoute] RingRequest ringRequest)
        {
            var result = await _ringService.GetRingByUidAsync(ringRequest.uid!);

            return Ok(result);
        }

        [HttpGet("serial/{serial}")]
        public async Task<ActionResult<RingResponse?>> GetRingBySerial([FromRoute] RingRequest ringRequest)
        {
            var result = await _ringService.GetRingBySerialAsync(ringRequest.serial!);

            return Ok(result);
        }

        [HttpPost("add-media/{id}")]
        public async Task<ActionResult> AddMedia(string id)
        {
            return Ok();
        }
    }

}
