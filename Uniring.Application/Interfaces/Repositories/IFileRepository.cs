using Uniring.Domain.Entities;

namespace Uniring.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<FileRecord?> GetFileByIdAsync(Guid id);
    }
}
