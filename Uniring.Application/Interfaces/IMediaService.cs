using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Media;
using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces
{
    public interface IMediaService
    {
        Task<Result<MediaRequest>> SaveFileAsync(MediaRequest request);
        Task<Media?> GetMetadataAsync(Guid id);
        Task<Stream?> OpenReadStreamAsync(Guid id, CancellationToken ct = default);
        Task<bool> DeleteFileAsync(Guid id);
    }
}
