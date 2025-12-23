using Uniring.Contracts.Media;
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces
{
    public interface IMediaService
    {
        Task<bool> SaveFileAsync(MediaRequest file);
        Task<Stream?> OpenReadStreamAsync(FileRecord record, CancellationToken ct = default);
        Task<bool> DeleteFileAsync(FileRecord record);
    }
}
