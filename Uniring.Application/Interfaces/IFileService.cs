using Microsoft.AspNetCore.Http;
using Uniring.Contracts.File;
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces
{
    public interface IFileService
    {
        Task<bool> SaveFileAsync(FileRequest file);
        Task<Stream?> OpenReadStreamAsync(FileRecord record, CancellationToken ct = default);
        Task<bool> DeleteFileAsync(FileRecord record);
    }
}
