using Microsoft.Extensions.Hosting;
using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Application.Utils;
using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Media;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IHostEnvironment _hostEnvironment;

        public MediaService(IMediaRepository mediaRepository, IHostEnvironment hostEnvironment)
        {
            _mediaRepository = mediaRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Stream?> OpenReadStreamAsync(Guid id, CancellationToken ct = default)
        {
            var media = await _mediaRepository.GetFileByIdAsync(id);
            if (media == null || !File.Exists(media.Path))
                return null;

            try
            {
                return new FileStream(media.Path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Media?> GetMetadataAsync(Guid id)
        {
            return await _mediaRepository.GetFileByIdAsync(id);
        }

        public async Task<bool> DeleteFileAsync(Guid id)
        {
            var media = await _mediaRepository.GetFileByIdAsync(id);
            if (media == null)
                return false;

            try
            {
                // Delete physical file
                if (File.Exists(media.Path))
                    File.Delete(media.Path);

                // Remove from DB
                return await _mediaRepository.DeleteFileAsync(media);
            }
            catch
            {
                return false;
            }
        }

        public async Task<Result<MediaRequest>> SaveFileAsync(MediaRequest request)
        {
            if (request?.file == null)
                return Result<MediaRequest>.Error("Null file.");

            var file = request.file;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Validate media type
            if (!MediaTypeHandler.isValidMedia(ext))
                return Result<MediaRequest>.Error("Not Supported media type."); ;

            // Determine subfolder
            string subfolder = MediaTypeHandler.isValidImage(ext) ? "image" : "video";
            string uploadDir = Path.Combine(_hostEnvironment.ContentRootPath, "media", subfolder);
            Directory.CreateDirectory(uploadDir); // Ensure directory exists

            // Generate unique filename
            Guid id = Guid.NewGuid();
            string uniqueFileName = $"{id}{ext}";
            string relativePath = Path.Combine("media", subfolder, uniqueFileName);
            string fullPath = Path.Combine(uploadDir, uniqueFileName);

            // Save file to disk
            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch
            {
                return Result<MediaRequest>.Error("Failed to save file on disk."); ;
            }

            // Save metadata to DB
            var mediaRecord = new Media
            {
                Id = id,
                OriginalFileName = file.FileName,
                ContentType = file.ContentType ?? "application/octet-stream",
                Size = file.Length,
                Path = relativePath,
                CreatedAt = DateTime.UtcNow
            };

            bool dbSaved = await _mediaRepository.SaveFileAsync(mediaRecord);
            if (!dbSaved)
            {
                // Optional: Clean up file if DB save fails
                try { File.Delete(fullPath); } catch { }
                return Result<MediaRequest>.Error("Failed to save file on DB."); ;
            }

            return Result<MediaRequest>.Success(new MediaRequest { id = id });
        }
    }
}
