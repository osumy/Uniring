using Uniring.Application.Interfaces;
using Uniring.Contracts.File;
using Uniring.Domain.Entities;

namespace Uniring.Application.Services
{
    public class FileService : IFileService
    {
        public Task<bool> DeleteFileAsync(FileRecord record)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> OpenReadStreamAsync(FileRecord record, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveFileAsync(FileRequest file)
        {
            throw new NotImplementedException();
        }
    }
}
