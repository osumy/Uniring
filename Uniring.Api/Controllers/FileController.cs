using Microsoft.AspNetCore.Mvc;

namespace Uniring.Api.Controllers
{
    public class FileController : ApiControllerBase
    {
        [HttpGet("download-video")]
        public async Task<IActionResult> GetVideoById(Guid id)
        {
            return Ok();
        }

        [HttpGet("download-image")]
        public async Task<IActionResult> GetImageById(Guid id)
        {
            return Ok();
        }

        [HttpPost("upload-video")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            return Ok();
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            return Ok();
        }
    }
}
