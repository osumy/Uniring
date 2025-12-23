using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Application.Utils;
using Uniring.Contracts.Media;

namespace Uniring.Api.Controllers
{
    public class MediaController : ApiControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }


        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile(MediaRequest media)
        {
            if (media == null) return BadRequest("No file provided.");

            string ext = Path.GetExtension(media.file.FileName);
            if (!MediaTypeHandler.isValidMedia(ext)) return BadRequest("Unsupported image format.");

            bool res = await _mediaService.SaveFileAsync(media);

            if (!res) return StatusCode(500);

            return Created();
        }

        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadFileById([FromRoute] MediaRequest file)
        {

            //var stream = await _mediaService.OpenReadStreamAsync(file);
            //if (stream == null) return NotFound();
            //return File(stream, file.file.ContentType, enableRangeProcessing: false, fileDownloadName: record.OriginalFileName);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteFileById([FromRoute] MediaRequest file)
        {
            return Ok();
        }
    }
}
