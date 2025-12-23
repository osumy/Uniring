using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces.Repositories
{
    public interface IMediaRepository
    {
        Task<Media?> GetFileByIdAsync(Guid id);
        Task<bool> DeleteFileAsync(Media record);
        Task<bool> SaveFileAsync(Media record);
    }
}
