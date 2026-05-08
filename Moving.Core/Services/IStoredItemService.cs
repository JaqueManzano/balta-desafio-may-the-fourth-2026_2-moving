using Moving.Core.Models;

namespace Moving.Core.Services
{
    public interface IStoredItemService
    {
        Task<IReadOnlyList<StoredItem>> GetByBoxIdAsync(Guid boxId);
        Task<StoredItem?> GetByIdAsync(Guid itemId);
        Task<StoredItem?> CreateAsync(Guid boxId, StoredItem item);
        Task<StoredItem?> UpdateAsync(StoredItem item);
        Task<bool> DeleteAsync(Guid itemId);
    }
}
