using Moving.Core.Models;

namespace Moving.Core.Services
{
    public interface IStorageBoxService
    {
        Task<StorageBox> CreateAsync(StorageBox box);
        Task<StorageBox?> GetByIdAsync(Guid id);
        Task<IEnumerable<StorageBox>> GetAllAsync();
        Task UpdateAsync(StorageBox box);
        Task DeleteAsync(Guid id);
        Task<StorageBox?> GetByDescriptionAsync(string description);
    }
}
