using Uniring.Application.Interfaces;
using Uniring.Contracts.Media;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class MediaService : IMediaService
    {
        public Task<bool> DeleteFileAsync(FileRecord record)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> OpenReadStreamAsync(FileRecord record, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveFileAsync(MediaRequest file)
        {
            throw new NotImplementedException();
        }
    }
}
