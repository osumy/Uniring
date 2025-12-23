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
            if (media?.file == null)
                return BadRequest("No file provided.");

            string ext = Path.GetExtension(media.file.FileName);
            if (!MediaTypeHandler.isValidMedia(ext))
                return BadRequest("Unsupported media format.");

            var res = await _mediaService.SaveFileAsync(media);
            if (!res.IsSuccess)
                return StatusCode(500, "Failed to save file.");

            return Ok(res.Data);
        }


        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadFileById(Guid id)
        {
            var stream = await _mediaService.OpenReadStreamAsync(id);
            if (stream == null)
                return NotFound();

            // Get metadata to set filename and content-type
            var media = await _mediaService.GetMetadataAsync(id);
            if (media == null)
            {
                stream.Dispose();
                return NotFound();
            }

            return File(stream, media.ContentType, media.OriginalFileName, enableRangeProcessing: true);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteFileById(Guid id)
        {
            bool success = await _mediaService.DeleteFileAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
