using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Application.Utils;
using Uniring.Contracts.File;
using static System.Net.Mime.MediaTypeNames;

namespace Uniring.Api.Controllers
{
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }


        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile(FileRequest file)
        {
            if (file == null) return BadRequest("No file provided.");
            string ext = Path.GetExtension(file.file.FileName);
            if (!MediaTypeHandler.isValidMedia(ext)) return BadRequest("Unsupported image format.");

            bool res = await _fileService.SaveFileAsync(file);

            if (!res) return StatusCode(500);

            return Created();
        }

        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadFileById([FromRoute] FileRequest file)
        {

            //var stream = await _fileService.OpenReadStreamAsync(file);
            //if (stream == null) return NotFound();
            //return File(stream, file.file.ContentType, enableRangeProcessing: false, fileDownloadName: record.OriginalFileName);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteFileById([FromRoute] FileRequest file)
        {
            return Ok();
        }
    }
}
